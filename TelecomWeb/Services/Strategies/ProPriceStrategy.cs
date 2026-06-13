namespace Telecom.Services.Strategies
{
    public class ProPriceStrategy : IPriceStrategy
    {
        public double CalculatePrice(double basePrice)
        {
            return basePrice * 0.8;
        }
    }
}