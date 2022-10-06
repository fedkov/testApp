using CheckoutBLL.Abstract.DTOs;
using CheckoutBLL.Abstract.Exceptions;
using CheckoutBLL.Abstract.Interfaces;


namespace CheckoutBLL.Implementation
{
    public class OrderTotalCalculator : IOrderTotalCalculator
    {
        private readonly ISupermarketOfferProvider _supermarketOfferProvider;

        public OrderTotalCalculator(ISupermarketOfferProvider supermarketOfferProvider)
        {
            _supermarketOfferProvider = supermarketOfferProvider;
        }

        /// <inheritdoc/>
        public decimal CalculateTotal(Order order)
        {
            ValidateOrder(order);

            decimal result = 0; //order total

            //Calculate total
            var currentOffer = _supermarketOfferProvider.GetCurrent();

            foreach (var productOrder in order.OrderItems)
            {
                var productOffer = currentOffer.Products.FirstOrDefault(x => x.ProductId == productOrder.ProductId);
                if (productOffer == null)
                    throw new ProductOutOfStockException(productOrder.ProductId);

                decimal productTotal = 0;
                
                var remainingAmount = productOrder.Amount;

                var ruleDescOrder = productOffer.PriceRules
                        .OrderByDescending(r => r.Amount)
                        .ToList();

                var ruleForSmallestUnit = ruleDescOrder.Last();

                while (remainingAmount > 0)
                {
                    var rule = ruleDescOrder
                        .Where(r => r.Amount <= remainingAmount)
                        .FirstOrDefault();

                    if (rule != null)
                    {
                        productTotal += rule.Price; // price for {rule.Amount}
                        remainingAmount -= rule.Amount; // decrease {remainingAmount} by {rule.Amount}
                    }
                    else
                    {
                        productTotal += ruleForSmallestUnit.Price / ruleForSmallestUnit.Amount * remainingAmount;
                        break;
                    }
                }

                result += productTotal;
            }

            return result;
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOrderException"></exception>
        private void ValidateOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (order.OrderItems == null || order.OrderItems.Any(x => x.Amount == 0 || String.IsNullOrEmpty(x.ProductId)))
                throw new InvalidOrderException();
        }
    }
}
