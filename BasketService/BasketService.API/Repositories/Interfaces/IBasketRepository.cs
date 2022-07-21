namespace BasketService.API.Repositories.Interfaces
{
    public interface IBasketRepository<T>
    {
        public T? GetStringKey(string key);
        public T UpdateStringKey(string key, T entity);
        public T? DeleteStringKey(string key);
    }
}