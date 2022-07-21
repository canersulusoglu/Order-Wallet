namespace OrderService.API.ViewModels
{
    public class UserOrderViewModel
    {
        [JsonProperty]
        public string UserEmail { get; set; }

        [JsonProperty]
        public List<UserOrderItemViewModel> UserOrderItems { get; set; }

        [JsonProperty]
        public DateTime OrderDate { get; set; }
    }
}
