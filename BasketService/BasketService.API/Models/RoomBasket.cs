namespace BasketService.API.Models
{
    public class RoomBasket
    {
        public string? OrderId { get; set; }
        public string? OrderMessage { get; set; }
        public string ConfirmedBasketUserEmail { get; set; }
        public string ConfirmedBasketUserName { get; set; }
        public string RoomName { get; set; }
        public List<UserBasket> Users { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
