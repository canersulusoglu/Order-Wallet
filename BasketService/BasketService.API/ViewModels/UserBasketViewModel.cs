namespace BasketService.API.ViewModels
{
    public class UserBasketViewModel
    {
        [JsonProperty]
        public string UserEmail { get; set; }

        [JsonProperty]
        public List<UserBasketItemViewModel> UserBasketItems { get; set; }

        [JsonProperty]
        public DateTime ? OrderDate  { get; set; }

    }
}
