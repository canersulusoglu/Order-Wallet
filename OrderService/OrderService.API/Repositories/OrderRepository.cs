namespace OrderService.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private DatabaseContext RepositoryContext { get; set; }
        public OrderRepository(DatabaseContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public IQueryable<Models.Order> FindAll() => RepositoryContext.Set<Models.Order>().Include(x => x.OrderItems).AsNoTracking();
        public IQueryable<Models.Order> FindByCondition(Expression<Func<Models.Order, bool>> expression) => RepositoryContext.Set<Models.Order>().Where(expression).Include(x => x.OrderItems).AsNoTracking();
        public IQueryable<Models.Order> FindByConditionWithLimitAndOffset(Expression<Func<Models.Order, bool>> expression, int limit, int offset) => RepositoryContext.Set<Models.Order>().Where(expression).Skip(offset).Take(limit).OrderByDescending(x => x.CreatedTime).Include(x => x.OrderItems).AsNoTracking();
        public IQueryable<Models.Order> FindWithLimitAndOffset(int limit, int offset) => RepositoryContext.Set<Models.Order>().Skip(offset).Take(limit).OrderByDescending(x => x.CreatedTime).Include(x => x.OrderItems).AsNoTracking();
        public void Create(Models.Order entity) => RepositoryContext.Set<Models.Order>().Add(entity);
        public void Update(Models.Order entity) => RepositoryContext.Set<Models.Order>().Update(entity);
        public void Delete(Models.Order entity) => RepositoryContext.Set<Models.Order>().Remove(entity);
    }
}
