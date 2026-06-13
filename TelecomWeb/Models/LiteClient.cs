namespace Telecom.Models
{
    public class LiteClient : Client
    {
        public LiteClient(long phoneNumber, string name) : base(phoneNumber, name)
        {
            ActiveServices.Add("Basic Internet");
        }
    }
}