namespace Telecom.Models;

public class BadClientKey
{
    public long PhoneNumber { get; set; }
        
    public BadClientKey(long phone) => PhoneNumber = phone;
}

public class GoodClientKey
{
    public long PhoneNumber { get; }
        
    public GoodClientKey(long phone) => PhoneNumber = phone;

    public override bool Equals(object? obj)
    {
        if (obj is GoodClientKey other)
            return PhoneNumber == other.PhoneNumber;
                
        return false;
    }

    public override int GetHashCode()
    {
        return PhoneNumber.GetHashCode();
    }

    public override string ToString() => PhoneNumber.ToString();
}