using Newtonsoft.Json;

namespace CheckoutBLL.Abstract.DTOs
{
    public class PriceRule
    {
        [JsonProperty("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Countable (per piece) or decimal (in kilogram) number
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}
