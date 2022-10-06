using CheckoutBLL.Abstract.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheckoutBLL.Abstract.Exceptions;

namespace CheckoutBLL.Abstract.Interfaces
{
    public interface IOrderTotalCalculator
    {
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOrderException"></exception>
        /// <exception cref="ProductOutOfStockException"></exception>
        decimal CalculateTotal(Order order);
    }
}
