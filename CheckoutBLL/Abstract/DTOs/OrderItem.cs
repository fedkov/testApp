using Newtonsoft.Json;

namespace CheckoutBLL.Abstract.DTOs
{
    public class OrderItem
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}
