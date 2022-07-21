namespace OrderService.API.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private DatabaseContext RepositoryContext { get; set; }
        public OrderItemRepository(DatabaseContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }
        public IQueryable<Models.OrderItem> FindAll() => RepositoryContext.Set<Models.OrderItem>().AsNoTracking();
        public IQueryable<Models.OrderItem> FindByCondition(Expression<Func<Models.OrderItem, bool>> expression) => RepositoryContext.Set<Models.OrderItem>().Where(expression).AsNoTracking();
        public void Create(Models.OrderItem entity) => RepositoryContext.Set<Models.OrderItem>().Add(entity);
        public void Update(Models.OrderItem entity) => RepositoryContext.Set<Models.OrderItem>().Update(entity);
        public void Delete(Models.OrderItem entity) => RepositoryContext.Set<Models.OrderItem>().Remove(entity);
    }
}
