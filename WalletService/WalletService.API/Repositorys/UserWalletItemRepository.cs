using OrderService.API.Repositories.Interfaces;
using WalletService.API.Models;

namespace OrderService.API.Repositories
{
    public class UserWalletItemRepository :IUserWalletItemRepository
    {
        protected DatabaseContext _databaseContext { get; init; }
        public DbSet<UserWalletItem> RepositoryContext { get; init; }
        public UserWalletItemRepository(DatabaseContext databasecontext)
        {
            _databaseContext = databasecontext;
            RepositoryContext = databasecontext.Set<UserWalletItem>();
        }
        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
