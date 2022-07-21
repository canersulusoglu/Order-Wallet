namespace OrderService.API.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        public IQueryable<Models.OrderItem> FindAll();
        public IQueryable<Models.OrderItem> FindByCondition(Expression<Func<Models.OrderItem, bool>> expression);
        public void Create(Models.OrderItem entity);
        public void Update(Models.OrderItem entity);
        public void Delete(Models.OrderItem entity);
    }
}
