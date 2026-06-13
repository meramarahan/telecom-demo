using Telecom.Services.Strategies;
using Xunit;

namespace Telecom.Tests
{
    public class PriceStrategyTests
    {
        [Fact]
        public void LitePriceStrategy_CalculatePrice_ReturnsFullPrice()
        {
            // Позитивний сценарій: базова ціна не змінюється
            var strategy = new LitePriceStrategy();
            double basePrice = 100.0;
            
            double result = strategy.CalculatePrice(basePrice);
            
            Assert.Equal(100.0, result);
        }

        [Fact]
        public void ProPriceStrategy_CalculatePrice_ReturnsDiscountedPrice()
        {
            // Позитивний сценарій: застосовується знижка 20%
            var strategy = new ProPriceStrategy();
            double basePrice = 100.0;
            
            double result = strategy.CalculatePrice(basePrice);
            
            Assert.Equal(80.0, result); // 100 * 0.8 = 80
        }
    }
}