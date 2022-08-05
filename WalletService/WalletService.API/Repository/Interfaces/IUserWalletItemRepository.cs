using WalletService.API.Models;

namespace OrderService.API.Repositories.Interfaces
{
    public interface IUserWalletItemRepository
    {
        public DbSet<UserWalletItem> RepositoryContext { get; init; }
        public Task SaveChanges();
    }
}
