using PaymentWebApi.Dtos.MercadoPago;
using System.Text.Json;

namespace PaymentWebApi.MercadoPagoServices;

public class MerchantOrderService : MercadoPagoService, IMerchantOrderService
{
    //TODO: Salvar em configurações.
    private readonly string GET_PAYMENT_URL = "https://api.mercadopago.com/v1/payments/";

    public async Task<MerchantOrderDto?> GetMerchantOrderAsync(string merchantOrderUrl)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(merchantOrderUrl);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MerchantOrderDto>(jsonResponse);
    }

    public async Task<PaymentInfoDto?> GetMerchantOrderPaymentAsync(string paymentId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(GET_PAYMENT_URL + paymentId);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        if (jsonResponse != null)
        {
            PaymentInfoDto? info = JsonSerializer.Deserialize<PaymentInfoDto>(jsonResponse);
            return info;
        }
        return null;
    }
}
