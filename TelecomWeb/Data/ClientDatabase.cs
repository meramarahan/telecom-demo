using Telecom.Collections;
using Telecom.Models;

namespace Telecom.Data
{
    public class ClientDatabase : IClientDatabase
    {
        private readonly CustomDictionary<long, Client> _clients = new CustomDictionary<long, Client>();
        private int _count;

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public void AddClient(Client client)
        {
            _clients.Add(client.PhoneNumber, client);
            _count++;
        }

        public void UpdateClient(Client client)
        {
        }

        public void AddPayment(long phone, double amount)
        {
        }

        public void AddClientService(long phone, int serviceId)
        {
        }

        public void RemoveClientService(long phone, int serviceId)
        {
        }

        public void RemoveClient(long phoneNumber)
        {
            _clients.Remove(phoneNumber);
            _count--;
        }

        public Client GetClient(long phone)
        {
            return _clients[phone];
        }

        public System.Collections.Generic.IEnumerable<Client> GetAllClients()
        {
            return _clients.Values;
        }

        public void UpdateTwoFactorAuth(long phone, bool isEnabled)
        {
        }
    }
}