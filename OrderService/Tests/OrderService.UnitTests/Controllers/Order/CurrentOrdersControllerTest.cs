namespace OrderService.UnitTests.Controllers.Order
{
    public class CurrentOrdersControllerTest
    {
        private readonly Mock<ICurrentOrdersService> _currentOrdersService;

        public CurrentOrdersControllerTest()
        {
            _currentOrdersService = new Mock<ICurrentOrdersService>();
        }

        [Fact]
        public void GetOrdersToUser_Success()
        {
            // Arrange
            List<UserOrderViewModel> fakeUserOrders = new List<UserOrderViewModel>
            {
                new UserOrderViewModel { UserEmail="fake_Email", UserOrderItems=new List<UserOrderItemViewModel>(), OrderDate=DateTime.UtcNow },
                new UserOrderViewModel { UserEmail="fake_Email", UserOrderItems=new List<UserOrderItemViewModel>(), OrderDate=DateTime.UtcNow },
                new UserOrderViewModel { UserEmail="fake_Email", UserOrderItems=new List<UserOrderItemViewModel>(), OrderDate=DateTime.UtcNow },
            };
            _currentOrdersService
                .Setup(x => x.GetUserCurrentOrders(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(fakeUserOrders);
            var currentOrdersController = new CurrentOrdersController(_currentOrdersService.Object);

            // Act
            var actionResult = currentOrdersController.GetCurrentOrdersOfUser();

            // Assert
            var statusOkActionResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(statusOkActionResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.IsType<List<UserOrderViewModel>>(statusOkActionResult.Value);
        }


        [Fact]
        public void GetOrdersToEmployee_Success()
        {
            // Arrange
            List<RoomOrderViewModel> fakeOrders = new List<RoomOrderViewModel>
            {
                new RoomOrderViewModel { RoomName="101", UserOrders=new List<UserOrderViewModel>() },
                new RoomOrderViewModel { RoomName="102", UserOrders=new List<UserOrderViewModel>() },
                new RoomOrderViewModel { RoomName="103", UserOrders=new List<UserOrderViewModel>() },
                new RoomOrderViewModel { RoomName="104", UserOrders=new List<UserOrderViewModel>() },
            };
            _currentOrdersService
                .Setup(x => x.GetAllCurrentOrders())
                .Returns(fakeOrders);
            var currentOrdersController = new CurrentOrdersController(_currentOrdersService.Object);

            // Act
            var actionResult = currentOrdersController.GetCurrentOrdersToEmployee();

            // Assert
            var statusOkActionResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(statusOkActionResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.IsType<List<RoomOrderViewModel>>(statusOkActionResult.Value);
        }

    }
}
