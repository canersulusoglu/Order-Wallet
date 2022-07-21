namespace OrderService.API.Repositories.Interfaces
{
    public interface ICacheOrderRepository<T>
    {
        public List<string> ScanKeys(string match);
        public T? GetStringKey(string key);
        public void AddStringKey(string key, T entity);
        public T? UpdateStringKey(string key, T entity);
        public T? DeleteStringKey(string key);
        public void SubscribeToChannel(string channel);
        public void UnSubscribeFromChannel(string channel);
    }
}
