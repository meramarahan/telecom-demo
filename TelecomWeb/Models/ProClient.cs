using System.Text.Json.Serialization;

namespace Telecom.Models
{
    public class ProClient : Client
    {
        public double InternetSpeed { get; init; }
        public double InternetGb { get; init; }

        [JsonConstructor]
        public ProClient(long phoneNumber, string name, double internetSpeed, double internetGb) 
            : base(phoneNumber, name)
        {
            InternetSpeed = internetSpeed;
            InternetGb = internetGb;
            ActiveServices.Add("Gigabit Internet");
            ActiveServices.Add("Premium TV");
        }
    }
}