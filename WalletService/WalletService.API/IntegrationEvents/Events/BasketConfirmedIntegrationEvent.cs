using EventBus.RabbitMQ.Events;

namespace WalletService.API.IntegrationEvents.Events
{
    public record BasketConfirmedIntegrationEvent : IntegrationEvent
    {
        public string RoomBasketAndOrder { get; set; }

    }
}
