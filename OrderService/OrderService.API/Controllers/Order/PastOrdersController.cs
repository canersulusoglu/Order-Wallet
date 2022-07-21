namespace OrderService.API.Controllers.Orders
{
    [Route("api/[controller]")]
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
        [ProducesResponseType(typeof(List<Order>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPastOrdersOfUserAsync()
        {
            try
            {
                var userEmail = "test@gmail.com";
                List<Order> userPastOrders = _pastOrdersService.GetUserPastOrders(userEmail);

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
        [ProducesResponseType(typeof(List<Order>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPastOrdersToEmployeeAsync(int itemsPerPage, int pageNumber)
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
