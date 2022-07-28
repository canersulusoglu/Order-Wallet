namespace BasketService.API.Controllers.Checkout
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        private readonly IIdentityService _identityService;

        public CheckoutController(ICheckoutService checkoutService, IIdentityService identityService)
        {
            _checkoutService = checkoutService;
            _identityService = identityService;
        }

        [HttpPost("checkoutBasket")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CheckoutBasketAsync(CheckoutBasketViewModel request)
        {
            try
            {
                string userEmail = _identityService.GetUserIdentity();
                string userName = _identityService.GetUserName();

                await _checkoutService.confirmBasket(userEmail, userName, request.OrderMessage);
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
