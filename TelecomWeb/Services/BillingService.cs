using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Telecom.Data;
using Telecom.Models;
using Telecom.Services.Strategies;

namespace Telecom.Services
{
    [ExcludeFromCodeCoverage]
    public class BillingService
    {
        private readonly IClientDatabase _db;
        private readonly BlockingCollection<Message> _queue = new BlockingCollection<Message>();

        public BillingService(IClientDatabase db)
        {
            _db = db;
            var thread = new Thread(ProcessMessages) { IsBackground = true };
            thread.Start();
        }

        public void Send(Message message)
        {
            _queue.Add(message);
        }

        private void ProcessMessages()
        {
            foreach (var msg in _queue.GetConsumingEnumerable())
            {
                try
                {
                    if (msg.Command == "TopUp")
                    {
                        dynamic payload = msg.Payload!;
                        long phone = payload.Phone;
                        double amount = payload.Amount;
                        
                        _db.AddPayment(phone, amount);
                        msg.OnSuccess?.Invoke(null);
                    }
                    else if (msg.Command == "Subscribe")
                    {
                        dynamic payload = msg.Payload!;
                        long phone = payload.Phone;
                        int serviceId = payload.ServiceId;
                        double basePrice = payload.Price;

                        var client = _db.GetClient(phone);

                        IPriceStrategy strategy;
                        if (client is ProClient)
                        {
                            strategy = new ProPriceStrategy();
                        }
                        else
                        {
                            strategy = new LitePriceStrategy();
                        }

                        double finalPrice = strategy.CalculatePrice(basePrice);

                        if (client.Balance < finalPrice)
                        {
                            throw new Exception("Not enough funds on balance.");
                        }

                        _db.AddPayment(phone, -finalPrice);
                        _db.AddClientService(phone, serviceId);

                        msg.OnSuccess?.Invoke(null);
                    }
                    else if (msg.Command == "Unsubscribe")
                    {
                        dynamic payload = msg.Payload!;
                        long phone = payload.Phone;
                        int serviceId = payload.ServiceId;
                        
                        _db.RemoveClientService(phone, serviceId);
                        msg.OnSuccess?.Invoke(null);
                    }
                    else
                    {
                        throw new Exception("Unknown command.");
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