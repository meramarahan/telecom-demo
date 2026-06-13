using Telecom.Models;
using Xunit;

namespace Telecom.Tests
{
    public class ClientModelsTests
    {
        [Fact]
        public void Deposit_AddsValueToBalance_AndRecordsHistory()
        {
            var client = new LiteClient(380991112233, "Test");

            client.Deposit(50.5);

            Assert.Equal(50.5, client.Balance);
            Assert.Contains(50.5, client.PaymentHistory);
        }

        [Fact]
        public void Deposit_IgnoresNegativeValues()
        {
            var client = new LiteClient(380991112233, "Test");

            client.Deposit(-10.0);

            Assert.Equal(0, client.Balance);
            Assert.Empty(client.PaymentHistory);
        }
    }
}