namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly ILifetimeScope _lifeTimeScope;
        private readonly int _retryCount;

        const string BROKER_NAME = "order_wallet_event_bus";
        const string AUTOFAC_SCOPE_NAME = "order_wallet_event_bus";
        private IModel _consumerChannel;
        private string _queueName;

        public EventBusRabbitMQ(IRabbitMQPersistentConnection rabbitMQPersistentConnection, IEventBusSubscriptionsManager subsManager, ILifetimeScope lifetimeScope, string queueName = null, int retryCount = 3)
        {
            _persistentConnection = rabbitMQPersistentConnection;
            _subsManager = subsManager;
            _lifeTimeScope = lifetimeScope;
            _retryCount = retryCount;
            _queueName = queueName;

            _consumerChannel = CreateConsumerChannel();
            _subsManager.OnEventRemoved += SubsManagerOnEventRemoved;
        }

        private void SubsManagerOnEventRemoved(object sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using var channel = _persistentConnection.CreateModel();
            channel.QueueUnbind(queue: _queueName,
                exchange: BROKER_NAME,
                routingKey: eventName
            );

            if (_subsManager.IsEmpty)
            {
                _queueName = string.Empty;
                _consumerChannel.Close();
            }
        }


        public void Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = RetryPolicy
                .Handle<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            var eventName = @event.GetType().Name;

            using var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");
            
            var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            });

            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                channel.BasicPublish(
                    exchange: BROKER_NAME,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body
                );
            });
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                _consumerChannel.QueueBind(
                    queue: _queueName,
                    exchange: BROKER_NAME,
                    routingKey: eventName
                );
            }

            _subsManager.AddSubscription<T, TH>();
            StartBasicConsume();
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                _consumerChannel.QueueBind(
                    queue: _queueName,
                    exchange: BROKER_NAME,
                    routingKey: eventName
                );
            }
            _subsManager.AddDynamicSubscription<TH>(eventName);
            StartBasicConsume();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            _subsManager.Clear();
        }


        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");

            channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private void StartBasicConsume()
        {
            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += ConsumerReceived;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer
                );
            }
        }

        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }

                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                
            }

            // Even on exception we take the message off the queue.
            // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
            // For more information see: https://www.rabbitmq.com/dlx.html
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }


        private async Task ProcessEvent(string eventName, string message)
        {
            //_logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using var scope = _lifeTimeScope.BeginLifetimeScope(AUTOFAC_SCOPE_NAME);
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        if (scope.ResolveOptional(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler) continue;
                        using dynamic eventData = JsonDocument.Parse(message);
                        await Task.Yield();
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var handler = scope.ResolveOptional(subscription.HandlerType);
                        if (handler == null) continue;
                        var eventType = _subsManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
            }
            else
            {
                //_logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }
    }
}
