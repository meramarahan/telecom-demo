using Telecom.Models;

namespace Telecom.Factories
{
    public static class ClientFactory
    {
        public static LiteClient CreateLite(long phone, string name)
        {
            return new LiteClient(phone, name);
        }

        public static ProClient CreatePro(long phone, string name, double speed, double gb)
        {
            return new ProClient(phone, name, speed, gb);
        }
    }
}