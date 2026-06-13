using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Telecom.Data;
using Telecom.Models;

namespace Telecom.Services
{
    [ExcludeFromCodeCoverage]
    public class ClientService
    {
        private readonly IClientDatabase _db;
        private readonly BlockingCollection<Message> _inbox = new BlockingCollection<Message>();

        public ClientService(IClientDatabase db)
        {
            _db = db;
            var thread = new Thread(ProcessMessages)
            {
                IsBackground = true,
                Name = "ClientServiceThread"
            };
            thread.Start();
        }

        public void Send(Message message)
        {
            _inbox.Add(message);
        }

        private void ProcessMessages()
        {
            foreach (var msg in _inbox.GetConsumingEnumerable())
            {
                try
                {
                    if (msg.Command == "Register")
                    {
                        var newClient = (Client)msg.Payload!;
                        _db.AddClient(newClient);
                        msg.OnSuccess?.Invoke(null);
                    }
                    else if (msg.Command == "Delete")
                    {
                        long delPhone = (long)msg.Payload!;
                        _db.RemoveClient(delPhone);
                        msg.OnSuccess?.Invoke(null);
                    }
                    else if (msg.Command == "GetAll")
                    {
                        var clients = _db.GetAllClients();
                        msg.OnSuccess?.Invoke(clients);
                    }
                    else if (msg.Command == "GetCount")
                    {
                        int count = _db.Count;
                        msg.OnSuccess?.Invoke(count);
                    }
                    else if (msg.Command == "Toggle2FA")
                    {
                        var payload = (dynamic)msg.Payload!;
                        long phone = payload.Phone;
                        bool enable = payload.Enable;

                        _db.UpdateTwoFactorAuth(phone, enable);

                        msg.OnSuccess?.Invoke(null);
                    }
                }
                catch (Exception ex)
                {
                    msg.OnError?.Invoke(ex);
                }
            }
        }
    }
}