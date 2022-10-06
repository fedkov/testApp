using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutBLL.Abstract.DTOs
{
    public class Product
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("priceRules")]
        public PriceRule[] PriceRules { get; set; }
    }
}
