using PaymentWebApi.Dtos.MercadoPago;

namespace PaymentWebApi.MercadoPagoServices;

public interface IMerchantOrderService
{
    Task<MerchantOrderDto?> GetMerchantOrderAsync(string merchantOrderUrl);
    Task<PaymentInfoDto?> GetMerchantOrderPaymentAsync(string paymentId);
}
