namespace OrderService.API.Controllers.Orders
{
    [Route("api/Order/[controller]")]
    [ApiController]
    public class PastOrdersController : ControllerBase
    {
        private readonly IPastOrdersService _pastOrdersService;
        private readonly IIdentityService _identityService;

        public PastOrdersController(IPastOrdersService pastOrdersService, IIdentityService identityService)
        {
            _pastOrdersService = pastOrdersService;
            _identityService = identityService;
        }

        [Route("getPastOrdersOfUser")]
        [HttpPost]
        [ProducesResponseType(typeof(Response<List<RoomOrder>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetPastOrdersOfUserAsync(int itemsPerPage, int pageNumber)
        {
            try
            {
                string userEmail = _identityService.GetUserIdentity();
         
                List<RoomOrder> userPastOrders = _pastOrdersService.GetUserPastOrders(userEmail, itemsPerPage, pageNumber);

                return Ok(new Response<List<RoomOrder>>
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
        [ProducesResponseType(typeof(Response<List<RoomOrder>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetPastOrdersToEmployeeAsync(int itemsPerPage, int pageNumber)
        {
            try
            {
                string userEmail = _identityService.GetUserIdentity();

                List<RoomOrder> allPastOrders = _pastOrdersService.GetAllPastOrders(itemsPerPage, pageNumber);
                
                return Ok(new Response<List<RoomOrder>>
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
