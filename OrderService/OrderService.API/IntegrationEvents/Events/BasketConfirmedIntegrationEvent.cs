namespace OrderService.API.IntegrationEvents.Events
{
    public record BasketConfirmedIntegrationEvent : IntegrationEvent
    {
        public string RoomOrder { get; init; }

        public BasketConfirmedIntegrationEvent(string roomOrder) => RoomOrder = roomOrder;
    }
}
