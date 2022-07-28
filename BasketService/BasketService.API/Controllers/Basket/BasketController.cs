namespace BasketService.API.Controllers.BasketController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService; // Redis
        private readonly IIdentityService _identityService;

        public BasketController(IBasketService basketService, IIdentityService identityService)
        {
            _basketService = basketService;
            _identityService = identityService;
        }

        [HttpPost("getBasket")]
        [ProducesResponseType(typeof(Response<RoomBasket>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetBasket()
        {
            try
            {
                string userEmail = _identityService.GetUserIdentity();
                RoomBasket userBasket = await _basketService.GetBasket(userEmail);

                return Ok(new Response<RoomBasket>
                {
                    isSuccess = true,
                    data = userBasket
                });
            }
            catch (BasketNotFoundException ex)
            {
                return Ok(new Response
                {
                    isSuccess = false,
                    messageCode = MessageCodes.BasketNotFound
                });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("updateBasket")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateBasket(UpdateBasketViewModel request)
        {
            try
            {
                string userEmail = _identityService.GetUserIdentity();
                string userName = _identityService.GetUserName();
                string userRoomName = _identityService.GetUserRoomName();

                await _basketService.UpdateBasket(userRoomName, userEmail, userName, request);
                return Ok(new Response
                {
                    isSuccess=true
                });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("deleteBasket")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteBasket()
        {
            try
            {
                string userEmail = _identityService.GetUserIdentity();
                await _basketService.DeleteBasket(userEmail);
                return Ok(new Response
                {
                    isSuccess=true
                });
            }
            catch(BasketNotFoundException ex)
            {
                return Ok(new Response
                {
                    isSuccess=false,
                    messageCode=MessageCodes.BasketNotFound
                });
            }
            catch
            {
                return BadRequest();
            }
            
        }


    }


}
