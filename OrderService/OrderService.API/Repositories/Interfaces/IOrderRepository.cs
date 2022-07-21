namespace OrderService.API.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public IQueryable<Models.Order> FindAll();
        public IQueryable<Models.Order> FindByCondition(Expression<Func<Models.Order, bool>> expression);
        public IQueryable<Models.Order> FindByConditionWithLimitAndOffset(Expression<Func<Models.Order, bool>> expression, int limit, int offset);
        public IQueryable<Models.Order> FindWithLimitAndOffset(int limit, int offset);
        public void Create(Models.Order entity);
        public void Update(Models.Order entity);
        public void Delete(Models.Order entity);
    }
}
