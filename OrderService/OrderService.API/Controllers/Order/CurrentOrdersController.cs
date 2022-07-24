namespace OrderService.API.Controllers.Orders
{
    [Route("api/Order/[controller]")]
    [ApiController]
    public class CurrentOrdersController : ControllerBase
    {
        private readonly ICurrentOrdersService _currentOrdersService;

        public CurrentOrdersController(ICurrentOrdersService currentOrdersService)
        {
            _currentOrdersService = currentOrdersService;
        }

        [Route("getCurrentOrdersOfUser")]
        [HttpPost]
        [ProducesResponseType(typeof(Response<List<UserOrderViewModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCurrentOrdersOfUser()
        {
            try
            {
                /*
                /* Identity Here
                var userId, userRoom = _identityService.GetUserIdentity();
                */
                string userId = "300";
                string roomName = "500";
                List<UserOrderViewModel> userOrders = await _currentOrdersService.GetUserCurrentOrders(userId);
                return Ok(new Response<List<UserOrderViewModel>>
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
        [ProducesResponseType(typeof(Response<List<UserOrderViewModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCurrentUserOrdersToEmployee()
        {
            try
            {
                /*
                /* Identity Here
                var userId, userRoom = _identityService.GetUserIdentity();
                */
                List<UserOrderViewModel> allCurrentOrders = await _currentOrdersService.GetAllCurrentUserOrders();
                return Ok(new Response<List<UserOrderViewModel>>
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

        [Route("getCurrentRoomOrdersToEmployee")]
        [HttpPost]
        [ProducesResponseType(typeof(Response<List<RoomOrderViewModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCurrentRoomOrdersToEmployee()
        {
            try
            {
                /*
                /* Identity Here
                var userId, userRoom = _identityService.GetUserIdentity();
                */
                List<RoomOrderViewModel> allRoomOrders = await _currentOrdersService.GetAllCurrentRoomOrders();
                return Ok(new Response<List<RoomOrderViewModel>>
                {
                    isSuccess = true,
                    data = allRoomOrders
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
                /*
                /* Identity Here
                var userId, userRoom = _identityService.GetUserIdentity();
                */
                string employeeId = "600";
                await _currentOrdersService.FinishUserOrder(orderId, employeeId);
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
                /*
                /* Identity Here
                var userId, userRoom = _identityService.GetUserIdentity();
                */
                string employeeId = "600";
                await _currentOrdersService.FinishUserOrders(orderIds, employeeId);
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
