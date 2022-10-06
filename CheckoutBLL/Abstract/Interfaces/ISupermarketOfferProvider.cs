using CheckoutBLL.Abstract.DTOs;

namespace CheckoutBLL.Abstract.Interfaces
{
    public interface ISupermarketOfferProvider
    {
        SupermarketOffer GetCurrent();
    }
}
