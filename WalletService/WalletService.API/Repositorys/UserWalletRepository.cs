using OrderService.API.Repositories.Interfaces;
using WalletService.API.Models;

namespace OrderService.API.Repositories
{
    public class UserWalletRepository : IUserWalletRepository
    {
        protected DatabaseContext _databaseContext { get; init; }
        public DbSet<UserWallet> RepositoryContext { get; init; }

        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
