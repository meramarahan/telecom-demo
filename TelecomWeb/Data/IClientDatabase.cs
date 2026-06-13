using Telecom.Models;

namespace Telecom.Data
{
    public interface IClientDatabase
    {
        int Count { get; }
        void AddClient(Client client);
        void RemoveClient(long phone);
        Client GetClient(long phone);
        IEnumerable<Client> GetAllClients();
        void UpdateClient(Client client);
        void AddPayment(long phone, double amount);
        void AddClientService(long phone, int serviceId);
        void RemoveClientService(long phone, int serviceId);
        void UpdateTwoFactorAuth(long phone, bool isEnabled);
    }
}