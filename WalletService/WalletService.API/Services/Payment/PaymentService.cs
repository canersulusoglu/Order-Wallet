using OrderService.API.Repositories;
using OrderService.API.Repositories.Interfaces;
using WalletService.API.Models;
using static OrderService.API.Repositories.UserWalletRepository;

namespace WalletService.API.Services.Payment
{
    public class PaymentService : IPaymentService
    {

        private readonly IUserWalletRepository _walletRepository;
        private readonly IUserWalletItemRepository _walletItemRepository;

        public PaymentService(IUserWalletRepository walletRepository, IUserWalletItemRepository walletItemRepository)
        {
            _walletRepository = walletRepository;
            _walletItemRepository = walletItemRepository;
        }

        public PaymentService(IUserWalletItemRepository userWalletItemRepository)
        {
            this.userWalletItemRepository = userWalletItemRepository;
        }

        public void getPaidOrderIdsOfUsersEmployee()
        {
            throw new NotImplementedException();
        }

        public void getTotalDebtOfUser()
        {
            throw new NotImplementedException();
        }

        public void getTotalDebtOfUsersToEmployee()
        {
            throw new NotImplementedException();
        }

        public void getUnpaidOrderIdsOfUsersEmployee()
        {
            throw new NotImplementedException();
        }

        public void payAllOrdersByEmployee()
        {
            throw new NotImplementedException();
        }

        public void payOrderByEmployee()
        {
            throw new NotImplementedException();


        }
        //string DebtAmountst = UserWallet.DebtAmount.ToString();
        public List<UserWallet> getUnpaidOrderIdsOfUsersEmployee(string userEmail, int itemsPerPage, int pageNumber)
        {
            int offset = (pageNumber == 1) ? 0 : (pageNumber - 1) * itemsPerPage;
            List<UserWallet> orders = _walletRepository.RepositoryContext.Include(x => x.UserEmail)
                                                                         .ThenInclude(x => x.DebtAmount)
                                                                         .Where(x => x.Users.Any(a => a.UserEmail == userEmail))
                                                                         .Skip(offset)
                                                                         .Take(itemsPerPage)
                                                                         .ToList();
            return orders;
        }

        public List<RoomOrder> GetAllPastOrders(int itemsPerPage, int pageNumber)
        {
            int offset = (pageNumber == 1) ? 0 : (pageNumber - 1) * itemsPerPage;
            List<RoomOrder> orders =  _orderRepository.RepositoryContext.Include(x => x.Users).ThenInclude(x => x.Products).Skip(offset).Take(itemsPerPage).OrderByDescending(x => x.CreatedTime).ToList();
            return orders;
        }
    }
}
