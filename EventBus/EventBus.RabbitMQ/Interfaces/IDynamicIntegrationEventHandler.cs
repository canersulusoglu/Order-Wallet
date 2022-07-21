namespace EventBus.RabbitMQ.Interfaces
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
