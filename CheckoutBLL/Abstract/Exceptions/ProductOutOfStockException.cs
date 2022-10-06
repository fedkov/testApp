namespace CheckoutBLL.Abstract.Exceptions
{
    public class ProductOutOfStockException : Exception
    {
        public ProductOutOfStockException(string productId) : base($"Product '{productId}' is out of stock.")
        { 
        }
    }
}
