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
        [ProducesResponseType(typeof(Dictionary<string, UserOrderViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetCurrentOrdersOfUser()
        {
            try
            {
                /* Identity Here
                var userEmail = _identityService.GetUserIdentity();
                */
                var userEmail = ""; 
                var roomName = "";
                var userOrders = _currentOrdersService.GetUserCurrentOrders(userEmail, roomName);
                return Ok(userOrders);
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("getCurrentOrdersToEmployee")]
        [HttpPost]
        [ProducesResponseType(typeof(Dictionary<string, List<RoomOrderViewModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetCurrentOrdersToEmployee()
        {
            try
            {
                /*
                /* Identity Here
                var userEmail = _identityService.GetUserIdentity();
                */

                var allOrdersByRoomName = _currentOrdersService.GetAllCurrentOrders();

                return Ok(allOrdersByRoomName);
            }
            catch
            {
                return Ok();
            }
        }
    }
}
