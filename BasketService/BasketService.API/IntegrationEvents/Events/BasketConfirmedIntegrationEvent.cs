namespace BasketService.API.IntegrationEvents.Events
{
    public record BasketConfirmedIntegrationEvent : IntegrationEvent
    {
        public string RoomBasketAndOrder { get; set; }

    }

}
