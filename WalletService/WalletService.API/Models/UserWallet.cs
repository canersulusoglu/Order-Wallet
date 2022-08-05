namespace WalletService.API.Models
{
    public class UserWallet : BaseModel
    {
        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public string OrderID { get; set; }

        public double DebtAmount { get; set; }

        public double PaymentAmount { get; set; }

        public string DebtAmountst { get; set; }

        public ICollection<UserWalletItem> Products { get; set; }
    }
}
    
