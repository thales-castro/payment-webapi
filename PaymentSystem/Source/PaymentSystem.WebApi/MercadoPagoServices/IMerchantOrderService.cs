using PaymentSystem.WebApi.Dtos.MercadoPago;

namespace PaymentSystem.WebApi.MercadoPagoServices;

public interface IMerchantOrderService
{
    Task<MerchantOrderDto?> GetMerchantOrderAsync(string merchantOrderUrl);
    Task<PaymentInfoDto?> GetMerchantOrderPaymentAsync(string paymentId, string token);
}
