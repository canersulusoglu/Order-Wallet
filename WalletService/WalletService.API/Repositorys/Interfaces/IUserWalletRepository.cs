using WalletService.API.Models;

namespace OrderService.API.Repositories.Interfaces
{
    public interface IUserWalletRepository
    {
        public DbSet<UserWallet> RepositoryContext { get; init; }
        public Task SaveChanges();
    }
}
