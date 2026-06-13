namespace Telecom.Services.Strategies
{
    public class LitePriceStrategy : IPriceStrategy
    {
        public double CalculatePrice(double basePrice)
        {
            return basePrice;
        }
    }
}