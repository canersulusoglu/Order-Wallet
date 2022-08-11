
namespace WalletService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase  //controllerBase bir kütüphane
    {
        private readonly IPaymentService _paymentService;
        private readonly IIdentityService _identityService;

        public WalletController(IPaymentService paymentService, IIdentityService identityService)
        {
            _paymentService = paymentService;
            _identityService = identityService;
        }

        [Route("getTotalDebtofUser")]
        [HttpPost]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetPastOrdersOfUserAsync(int itemsPerPage, int pageNumber)
        {
            try
            {
                string userEmail = _identityService.GetUserIdentity();


                return Ok(new Response
                {
                    isSuccess = true
                });
            }
            catch
            {
                return NotFound();
            }
        }

        [Route("getTotalDebtofUsersToEmployee")]
        [HttpPost]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetPastOrdersToEmployeeAsync(int itemsPerPage, int pageNumber)
        {
            try
            {
                string userEmail = _identityService.GetUserIdentity();


                return Ok(new Response
                {
                    isSuccess = true
                });
            }
            catch
            {
                return NotFound();
            }



        }
    }
}
