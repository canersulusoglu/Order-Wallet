namespace WalletService.API.Models
{
    public class UserWalletItem : BaseModel
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductQuantity { get; set; }

        public UserWallet? UserOrder { get; set; }
    }
}
