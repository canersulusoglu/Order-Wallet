namespace OrderService.API.Controllers.Orders
{
    [Route("api/Order/[controller]")]
    [ApiController]
    public class PastOrdersController : ControllerBase
    {
        private readonly IPastOrdersService _pastOrdersService;

        public PastOrdersController(IPastOrdersService pastOrdersService)
        {
            _pastOrdersService = pastOrdersService;
        }

        [Route("getPastOrdersOfUser")]
        [HttpPost]
        [ProducesResponseType(typeof(Response<List<Order>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetPastOrdersOfUserAsync(int itemsPerPage, int pageNumber)
        {
            try
            {
                /*
                /* Identity Here
                var userId, userRoom = _identityService.GetUserIdentity();
                */
                var userId = "300";
                List<Order> userPastOrders = _pastOrdersService.GetUserPastOrders(userId, itemsPerPage, pageNumber);

                return Ok(new Response<List<Order>>
                {
                    isSuccess = true,
                    data = userPastOrders
                });
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("getPastOrdersToEmployee")]
        [HttpPost]
        [ProducesResponseType(typeof(Response<List<Order>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetPastOrdersToEmployeeAsync(int itemsPerPage, int pageNumber)
        {
            try
            {
                List<Order> allPastOrders = _pastOrdersService.GetAllPastOrders(itemsPerPage, pageNumber);
                
                return Ok(new Response<List<Order>>
                {
                    isSuccess = true,
                    data = allPastOrders
                });
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
