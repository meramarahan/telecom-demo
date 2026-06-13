using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Telecom.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(LiteClient), "lite")]
    [JsonDerivedType(typeof(ProClient), "pro")]
    public abstract class Client
    {
        public long PhoneNumber { get; init; }
        public string Name { get; init; }
        public double Balance { get; set; }

        public List<double> PaymentHistory { get; init; } = new List<double>();
        public List<string> ActiveServices { get; init; } = new List<string>();

        public class SecurityProfile
        {
            public bool TwoFactorEnabled { get; set; } = false;
            public string LastLoginIp { get; set; } = "192.168.1.1";
        }

        public SecurityProfile Security { get; init; } = new SecurityProfile();

        [JsonConstructor]
        protected Client(long phoneNumber, string name)
        {
            PhoneNumber = phoneNumber;
            Name = name;
        }

        public void Deposit(double value)
        {
            if (value > 0)
            {
                Balance += value;
                PaymentHistory.Add(value);
            }
        }

        [ExcludeFromCodeCoverage]
        public virtual void ShowInfo()
        {
            // Метод для консольного виводу
        }
    }
}