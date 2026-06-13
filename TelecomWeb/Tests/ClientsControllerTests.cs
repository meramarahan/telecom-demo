using Microsoft.AspNetCore.Mvc;
using Moq;
using Telecom.Data;
using Telecom.Models;
using TelecomWeb.Controllers;
using Xunit;

namespace Telecom.Tests
{
    public class ClientsControllerTests
    {
        [Fact]
        public void GetAllClients_ReturnsOkResult()
        {
            var mockDb = new Mock<IClientDatabase>();
            var clients = new List<Client> { new LiteClient(380991112233, "Ivan") };
            mockDb.Setup(db => db.GetAllClients()).Returns(clients);
            var controller = new ClientsController(mockDb.Object);

            var result = controller.GetAllClients();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetByPhone_ReturnsNotFound_WhenClientDoesNotExist()
        {
            var mockDb = new Mock<IClientDatabase>();
            mockDb.Setup(db => db.GetAllClients()).Returns(new List<Client>());
            var controller = new ClientsController(mockDb.Object);

            var result = controller.GetByPhone(12345);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}