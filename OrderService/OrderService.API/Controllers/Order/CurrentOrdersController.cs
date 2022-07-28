namespace OrderService.API.Controllers.Orders
{
    [Route("api/Order/[controller]")]
    [ApiController]
    public class CurrentOrdersController : ControllerBase
    {
        private readonly ICurrentOrdersService _currentOrdersService;
        private readonly IIdentityService _identityService;

        public CurrentOrdersController(ICurrentOrdersService currentOrdersService, IIdentityService identityService)
        {
            _currentOrdersService = currentOrdersService;
            _identityService = identityService;
        }

        [Route("getCurrentOrdersOfUser")]
        [HttpPost]
        [ProducesResponseType(typeof(Response<List<RoomOrder>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCurrentOrdersOfUser()
        {
            try
            {
                string userEmail = _identityService.GetUserIdentity();

                List<RoomOrder> userOrders = await _currentOrdersService.GetUserCurrentOrders(userEmail);
                return Ok(new Response<List<RoomOrder>>
                {
                    isSuccess = true,
                    data = userOrders
                });
            }
            catch(OrderNotFoundException ex)
            {
                return Ok(new Response
                {
                    isSuccess = false,
                    messageCode = MessageCodes.OrderNotFound
                });
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("getCurrentUserOrdersToEmployee")]
        [HttpPost]
        [ProducesResponseType(typeof(Response<List<RoomOrder>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCurrentUserOrdersToEmployee()
        {
            try
            {
                string userEmail = _identityService.GetUserIdentity();

                List<RoomOrder> allCurrentOrders = await _currentOrdersService.GetAllCurrentOrders();
                return Ok(new Response<List<RoomOrder>>
                {
                    isSuccess = true,
                    data = allCurrentOrders
                });
            }
            catch (OrderNotFoundException ex)
            {
                return Ok(new Response
                {
                    isSuccess = false,
                    messageCode = MessageCodes.OrderNotFound
                });
            }
            catch
            {
                return Ok();
            }
        }


        [Route("approveOrderByEmployee")]
        [HttpPost]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ApproveOrderByEmployee(string orderId)
        {
            try
            {
                string userEmail = _identityService.GetUserIdentity();

                await _currentOrdersService.FinishOrder(orderId, userEmail);
                return Ok(new Response
                {
                    isSuccess = true
                });
            }
            catch (OrderNotFoundException ex)
            {
                return Ok(new Response
                {
                    isSuccess = false,
                    messageCode = MessageCodes.OrderNotFound
                });
            }
            catch
            {
                return Ok();
            }
        }

        [Route("approveOrdersByEmployee")]
        [HttpPost]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ApproveOrdersByEmployee(string[] orderIds)
        {
            try
            {
                string userEmail = _identityService.GetUserIdentity();

                await _currentOrdersService.FinishOrders(orderIds, userEmail);
                return Ok(new Response
                {
                    isSuccess = true
                });
            }
            catch (OrderNotFoundException ex)
            {
                return Ok(new Response
                {
                    isSuccess = false,
                    messageCode = MessageCodes.OrderNotFound
                });
            }
            catch
            {
                return Ok();
            }
        }
    }
}
