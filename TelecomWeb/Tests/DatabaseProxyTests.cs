using System.Collections.Generic;
using Telecom.Data;
using Telecom.Models;
using Xunit;

namespace Telecom.Tests
{
    public class DatabaseProxyTests
    {
        private class SpyDatabase : IClientDatabase
        {
            public int CallCount { get; private set; }
            public int Count => 1;

            public void AddClient(Client client) { CallCount++; }
            public void RemoveClient(long phone) { CallCount++; }
            public Client GetClient(long phone) { CallCount++; return null!; }
            public IEnumerable<Client> GetAllClients() { CallCount++; return new List<Client>(); }
            public void UpdateClient(Client client) { CallCount++; }
            public void AddPayment(long phone, double amount) { CallCount++; }
            public void AddClientService(long phone, int serviceId) { CallCount++; }
            public void RemoveClientService(long phone, int serviceId) { CallCount++; }
            public void UpdateTwoFactorAuth(long phone, bool isEnabled) { CallCount++; }
        }

        [Fact]
        public void Proxy_ExecutesAllMethods()
        {
            var spy = new SpyDatabase();
            var proxy = new DatabaseProxy(spy);
            var client = new LiteClient(380123456789, "TestName");

            proxy.AddClient(client);
            proxy.RemoveClient(380123456789);
            proxy.GetClient(380123456789);
            proxy.GetAllClients();
            proxy.UpdateClient(client);
            proxy.AddPayment(380123456789, 100);
            proxy.AddClientService(380123456789, 1);
            proxy.RemoveClientService(380123456789, 1);
            proxy.UpdateTwoFactorAuth(380123456789, true);
            
            int count = proxy.Count;

            Assert.Equal(9, spy.CallCount);
            Assert.Equal(1, count);
        }
    }
}