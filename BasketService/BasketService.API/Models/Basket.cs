namespace BasketService.API.Models
{
    public class Basket
    {
        public string UserName { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public Basket(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal totalprice = 0;
                foreach (var item in Items)
                {
                    totalprice += item.Price * item.Quantity;
                }
                return totalprice;
            }
        }
    }
}
