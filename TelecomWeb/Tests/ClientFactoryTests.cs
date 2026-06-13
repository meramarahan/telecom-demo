using System;
using Telecom.Factories;
using Telecom.Models;
using Xunit;

namespace Telecom.Tests
{
    public class ClientFactoryTests
    {
        [Fact]
        public void CreateLite_ReturnsLiteClient()
        {
            var phone = 380123456789L;
            var client = ClientFactory.CreateLite(phone, "Ivan");

            Assert.NotNull(client);
            Assert.Equal("Ivan", client.Name);
            Assert.Equal(phone, client.PhoneNumber);
            Assert.IsType<LiteClient>(client);
        }

        [Fact]
        public void CreatePro_ReturnsProClient()
        {
            var phone = 380987654321L;
            var client = ClientFactory.CreatePro(phone, "Petro", 1000.0, 100.0);

            Assert.NotNull(client);
            Assert.Equal("Petro", client.Name);
            Assert.Equal(phone, client.PhoneNumber);
            Assert.Equal(1000.0, client.InternetSpeed);
            Assert.Equal(100.0, client.InternetGb);
            Assert.IsType<ProClient>(client);
        }
    }
}