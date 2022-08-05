namespace WalletService.API.Services.Payment
{
    public interface IPaymentService
    {
        public void payOrderByEmployee();

        //by using rabbitmq this part uses


        public void payAllOrdersByEmployee();
        public void getUnpaidOrderIdsOfUsersEmployee();
        public void getPaidOrderIdsOfUsersEmployee();
        public void getTotalDebtOfUser();

        public void getTotalDebtOfUsersToEmployee();



    }
}
