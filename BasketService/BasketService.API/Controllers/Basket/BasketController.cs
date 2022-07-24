namespace BasketService.API.Controllers.BasketController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService; // Redis

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpPost("getBasket")]
        [ProducesResponseType(typeof(Response<RoomBasketViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetBasket()
        {
            try
            {
                /*
                /* Identity Here
                var userId, userRoom = _identityService.GetUserIdentity();
                */
                string roomName = "500";
                RoomBasketViewModel roomBasket = await _basketService.GetBasket(roomName);

                return Ok(new Response<RoomBasketViewModel>
                {
                    isSuccess = true,
                    data = roomBasket
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

        [HttpPost("updateBasket")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateBasket(RoomBasketViewModel roomBasketViewModel)
        {
            try
            {
                /*
                /* Identity Here
                var userId, userRoom = _identityService.GetUserIdentity();
                */
                string roomName = "500";
                await _basketService.UpdateBasket(roomName, roomBasketViewModel);
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
                /*
                /* Identity Here
                var userId, userRoom = _identityService.GetUserIdentity();
                */
                string roomName = "500";
                await _basketService.DeleteBasket(roomName);
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
