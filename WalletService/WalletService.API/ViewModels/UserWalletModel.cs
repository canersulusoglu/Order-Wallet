using WalletService.API.Models;

namespace WalletService.API.ViewModels
{
    public class UserWalletModel
    {
        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public string OrderID { get; set; }

        public double DebtAmount { get; set; }

        public double PaymentAmount { get; set; }

        public ICollection<UserWalletItem> Products { get; set; }
    }
}
