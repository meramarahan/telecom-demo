using Telecom.Models;

namespace Telecom.Data
{
    public class DatabaseProxy : IClientDatabase
    {
        private readonly IClientDatabase _database;

        public DatabaseProxy(IClientDatabase database)
        {
            _database = database;
        }

        public int Count
        {
            get
            {
                return _database.Count;
            }
        }

        public void AddClient(Client client)
        {
            Console.WriteLine($"[PROXY LOG] AddClient: {client.PhoneNumber}");
            _database.AddClient(client);
        }

        public void RemoveClient(long phone)
        {
            Console.WriteLine($"[PROXY LOG] RemoveClient: {phone}");
            _database.RemoveClient(phone);
        }

        public Client GetClient(long phone)
        {
            Console.WriteLine($"[PROXY LOG] GetClient: {phone}");
            return _database.GetClient(phone);
        }

        public IEnumerable<Client> GetAllClients()
        {
            Console.WriteLine("[PROXY LOG] GetAllClients");
            return _database.GetAllClients();
        }

        public void UpdateClient(Client client)
        {
            Console.WriteLine($"[PROXY LOG] UpdateClient: {client.PhoneNumber}");
            _database.UpdateClient(client);
        }

        public void AddPayment(long phone, double amount)
        {
            Console.WriteLine($"[PROXY LOG] AddPayment: {phone}");
            _database.AddPayment(phone, amount);
        }

        public void AddClientService(long phone, int serviceId)
        {
            Console.WriteLine($"[PROXY LOG] AddClientService: {phone}");
            _database.AddClientService(phone, serviceId);
        }

        public void RemoveClientService(long phone, int serviceId)
        {
            Console.WriteLine($"[PROXY LOG] RemoveClientService: {phone}");
            _database.RemoveClientService(phone, serviceId);
        }

        public void UpdateTwoFactorAuth(long phone, bool isEnabled)
        {
            Console.WriteLine($"[PROXY LOG] UpdateTwoFactorAuth: {phone}");
            _database.UpdateTwoFactorAuth(phone, isEnabled);
        }
    }
}