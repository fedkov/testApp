using Newtonsoft.Json;

namespace CheckoutBLL.Abstract.DTOs
{
    public class SupermarketOffer
    {
        [JsonProperty("products")]
        public Product[] Products { get; set; }
    }
}
