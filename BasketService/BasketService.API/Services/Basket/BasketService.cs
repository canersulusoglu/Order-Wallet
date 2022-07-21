namespace BasketService.API.Services.Basket
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository<RoomBasketViewModel> _basketRepository;
     
        public BasketService(IBasketRepository<RoomBasketViewModel> basketRepository)
        {
            _basketRepository=basketRepository;
        }

        public Task<RoomBasketViewModel> DeleteBasket(string roomName)
        {
            var deletedBasket = _basketRepository.DeleteStringKey(roomName);
            if(deletedBasket == null)
            {
                throw new BasketNotFoundException();
            }
            return Task.FromResult(deletedBasket);

        }

        public Task<RoomBasketViewModel> GetBasket(string roomName)
        {
            var basket = _basketRepository.GetStringKey(roomName);
            if (basket == null)
            {
                throw new BasketNotFoundException();
            }
            return Task.FromResult(basket);
        }

        public Task<RoomBasketViewModel> UpdateBasket(RoomBasketViewModel roomBasketViewModel)
        {
            var basket = _basketRepository.UpdateStringKey(roomBasketViewModel.RoomName, roomBasketViewModel);

            return Task.FromResult(basket);
        }
    }
}
