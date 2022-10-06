using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutBLL.Abstract.DTOs
{
    public class Order
    {
        [JsonProperty("orderItems")]
        public OrderItem[] OrderItems { get; set; }
    }
}
