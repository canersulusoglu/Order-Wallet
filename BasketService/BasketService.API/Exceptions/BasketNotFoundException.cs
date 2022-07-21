namespace BasketService.API.Exceptions
{
    public class BasketNotFoundException : Exception
    {
        public BasketNotFoundException()
        {

        }

        public BasketNotFoundException(string message) : base(message)
        {

        }

        public BasketNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
