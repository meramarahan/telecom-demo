using Xunit;
using Telecom.Models;
using Telecom.Factories;

namespace Telecom.Tests
{
    public class ClientDatabaseTests
    {
        [Fact]
        public void CreateProClient_ShouldSetCorrectProperties()
        {
            // Arrange & Act
            var client = ClientFactory.CreatePro(380991234567, "Viktor", 100.0, 50.0);

            // Assert
            Assert.Equal("Viktor", client.Name);
            Assert.Equal(100.0, client.InternetSpeed);
            Assert.Contains("Gigabit Internet", client.ActiveServices);
        }
    }
}