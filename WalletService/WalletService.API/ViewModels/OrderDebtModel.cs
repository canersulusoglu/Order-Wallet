using WalletService.API.Requests;

namespace WalletService.API.ViewModels
{
    public class OrderDebtModel : BaseModel
    {
        public string UserEmail { get; set; }

        public string OrderID { get; set; }

        public double DebtAmount { get; set; }

        public double PaymentAmount { get; set; }
    }
}
