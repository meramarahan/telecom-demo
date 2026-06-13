using System.Diagnostics.CodeAnalysis;
using MySqlConnector;
using Telecom.Models;

namespace Telecom.Data
{
    [ExcludeFromCodeCoverage]
    public class MySqlClientDatabase : IClientDatabase
    {
        private readonly string _connectionString;

        public MySqlClientDatabase(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public int Count
        {
            get
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();
                using var cmd = new MySqlCommand("SELECT COUNT(*) FROM Clients", connection);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public void AddClient(Client client)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                string type = client is ProClient ? "Pro" : "Lite";
                using var cmd1 = new MySqlCommand("INSERT INTO Clients (PhoneNumber, Name, Balance, ClientType, InternetSpeed, InternetGb) VALUES (@p, @n, @b, @t, @speed, @gb)", connection, transaction);

                cmd1.Parameters.AddWithValue("@p", client.PhoneNumber);
                cmd1.Parameters.AddWithValue("@n", client.Name);
                cmd1.Parameters.AddWithValue("@b", client.Balance);
                cmd1.Parameters.AddWithValue("@t", type);

                if (client is ProClient proClient)
                {
                    cmd1.Parameters.AddWithValue("@speed", proClient.InternetSpeed);
                    cmd1.Parameters.AddWithValue("@gb", proClient.InternetGb);
                }
                else
                {
                    cmd1.Parameters.AddWithValue("@speed", DBNull.Value);
                    cmd1.Parameters.AddWithValue("@gb", DBNull.Value);
                }

                cmd1.ExecuteNonQuery();

                using var cmd2 = new MySqlCommand("INSERT INTO SecurityProfiles (PhoneNumber, TwoFactorEnabled, LastLoginIp) VALUES (@p, @tfa, @ip)", connection, transaction);

                cmd2.Parameters.AddWithValue("@p", client.PhoneNumber);
                cmd2.Parameters.AddWithValue("@tfa", client.Security.TwoFactorEnabled ? 1 : 0);
                cmd2.Parameters.AddWithValue("@ip", client.Security.LastLoginIp);

                cmd2.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void RemoveClient(long phone)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var cmd = new MySqlCommand("DELETE FROM Clients WHERE PhoneNumber = @p", connection);
            cmd.Parameters.AddWithValue("@p", phone);
            cmd.ExecuteNonQuery();
        }

        public Client GetClient(long phone)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var cmd = new MySqlCommand("SELECT * FROM Clients c JOIN SecurityProfiles s ON c.PhoneNumber = s.PhoneNumber WHERE c.PhoneNumber = @p", connection);
            cmd.Parameters.AddWithValue("@p", phone);

            Client client;
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read()) throw new Exception("Client not found.");

                string name = reader.GetString("Name");
                string type = reader.GetString("ClientType");
                double balance = reader.GetDouble("Balance");

                if (type == "Pro")
                {
                    int speed = reader.IsDBNull(reader.GetOrdinal("InternetSpeed")) ? 1000 : reader.GetInt32("InternetSpeed");
                    int gb = reader.IsDBNull(reader.GetOrdinal("InternetGb")) ? 500 : reader.GetInt32("InternetGb");
                    client = new ProClient(phone, name, speed, gb);
                }
                else
                {
                    client = new LiteClient(phone, name);
                }

                client.Balance = balance;
                client.Security.TwoFactorEnabled = reader.GetBoolean("TwoFactorEnabled");
                client.Security.LastLoginIp = reader.GetString("LastLoginIp");
            }

            using var cmdServices = new MySqlCommand("SELECT s.ServiceName FROM ClientServices cs JOIN Services s ON cs.ServiceId = s.ServiceId WHERE cs.PhoneNumber = @p", connection);
            cmdServices.Parameters.AddWithValue("@p", phone);
            using (var readerServices = cmdServices.ExecuteReader())
            {
                while (readerServices.Read())
                {
                    client.ActiveServices.Add(readerServices.GetString("ServiceName"));
                }
            }

            return client;
        }

        public IEnumerable<Client> GetAllClients()
        {
            var list = new List<Client>();
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var cmd = new MySqlCommand("SELECT c.PhoneNumber, c.Name, c.ClientType, c.Balance, c.InternetSpeed, c.InternetGb, sp.TwoFactorEnabled, sp.LastLoginIp FROM Clients c LEFT JOIN SecurityProfiles sp ON c.PhoneNumber = sp.PhoneNumber", connection);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    long phone = reader.GetInt64("PhoneNumber");
                    string name = reader.GetString("Name");
                    string type = reader.GetString("ClientType");
                    double balance = reader.GetDouble("Balance");

                    Client client;
                    if (type == "Pro")
                    {
                        int speed = reader.IsDBNull(reader.GetOrdinal("InternetSpeed")) ? 1000 : reader.GetInt32("InternetSpeed");
                        int gb = reader.IsDBNull(reader.GetOrdinal("InternetGb")) ? 500 : reader.GetInt32("InternetGb");
                        client = new ProClient(phone, name, speed, gb);
                    }
                    else
                    {
                        client = new LiteClient(phone, name);
                    }

                    client.Balance = balance;
                    client.Security.TwoFactorEnabled = reader.IsDBNull(reader.GetOrdinal("TwoFactorEnabled")) ? false : reader.GetBoolean("TwoFactorEnabled");
                    client.Security.LastLoginIp = reader.IsDBNull(reader.GetOrdinal("LastLoginIp")) ? "192.168.1.1" : reader.GetString("LastLoginIp");
                    list.Add(client);
                }
            }

            using var cmdServices = new MySqlCommand("SELECT cs.PhoneNumber, s.ServiceName FROM ClientServices cs JOIN Services s ON cs.ServiceId = s.ServiceId", connection);
            using (var readerServices = cmdServices.ExecuteReader())
            {
                while (readerServices.Read())
                {
                    long phone = readerServices.GetInt64("PhoneNumber");
                    string serviceName = readerServices.GetString("ServiceName");
                    var client = list.Find(c => c.PhoneNumber == phone);
                    if (client != null) client.ActiveServices.Add(serviceName);
                }
            }

            return list;
        }

        public void UpdateClient(Client client)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var cmd = new MySqlCommand("UPDATE Clients SET Balance = @b WHERE PhoneNumber = @p", connection);
            cmd.Parameters.AddWithValue("@b", client.Balance);
            cmd.Parameters.AddWithValue("@p", client.PhoneNumber);
            cmd.ExecuteNonQuery();
        }

        public void AddPayment(long phone, double amount)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var cmd = new MySqlCommand("INSERT INTO Payments (PhoneNumber, Amount) VALUES (@p, @a)", connection);
            cmd.Parameters.AddWithValue("@p", phone);
            cmd.Parameters.AddWithValue("@a", amount);
            cmd.ExecuteNonQuery();
        }

        public void AddClientService(long phone, int serviceId)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var cmd = new MySqlCommand("INSERT INTO ClientServices (PhoneNumber, ServiceId) VALUES (@p, @s)", connection);
            cmd.Parameters.AddWithValue("@p", phone);
            cmd.Parameters.AddWithValue("@s", serviceId);
            cmd.ExecuteNonQuery();
        }

        public void RemoveClientService(long phone, int serviceId)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var cmd = new MySqlCommand("DELETE FROM ClientServices WHERE PhoneNumber = @p AND ServiceId = @s", connection);
            cmd.Parameters.AddWithValue("@p", phone);
            cmd.Parameters.AddWithValue("@s", serviceId);
            cmd.ExecuteNonQuery();
        }

        public void UpdateTwoFactorAuth(long phone, bool isEnabled)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var cmd = new MySqlCommand("UPDATE SecurityProfiles SET TwoFactorEnabled = @tfa WHERE PhoneNumber = @p", connection);
            cmd.Parameters.AddWithValue("@tfa", isEnabled ? 1 : 0);
            cmd.Parameters.AddWithValue("@p", phone);
            cmd.ExecuteNonQuery();
        }
    }
}