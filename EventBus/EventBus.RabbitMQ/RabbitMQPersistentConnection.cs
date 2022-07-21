namespace EventBus.RabbitMQ
{
    public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly int _retryCount;
        private IConnection _connection;
        private bool _disposed;

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount)
        {
            _connectionFactory = connectionFactory;
            _retryCount = retryCount;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public bool TryConnect()
        {
            var policy = RetryPolicy
                .Handle<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            policy.Execute(() =>
            {
                _connection = _connectionFactory.CreateConnection();
            });

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                return true;
            }
            else
            {
                return false;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action.");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            try
            {
                _connection.ConnectionShutdown -= OnConnectionShutdown;
                _connection.CallbackException -= OnCallbackException;
                _connection.ConnectionBlocked -= OnConnectionBlocked;
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // Events
        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            Console.WriteLine("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            Console.WriteLine("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            Console.WriteLine("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }
    }
}
