namespace BasketService.API.Controllers.Checkout
{
    [Route("api/Checkout/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("checkoutBasket")]
        [ProducesResponseType(typeof(Response<RoomBasketViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CheckoutBasketAsync(string roomName)
        {
            try
            {
                await _checkoutService.confirmBasket(roomName);
                return Ok(new Response
                {
                    isSuccess = true
                });
            }
            catch (BasketNotFoundException ex)
            {
                return Ok(new Response
                {
                    isSuccess = false
                });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
