namespace BasketService.API.Controllers.Checkout
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> CheckoutBasketAsync()
        {
            try
            {
                /*
                /* Identity Here
                var userId, userRoom = _identityService.GetUserIdentity();
                */
                string userId = "userTest";
                string roomName = "500";
                await _checkoutService.confirmBasket(userId, roomName);
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
