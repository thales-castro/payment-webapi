using PaymentSystem.WebApi.Dtos.MercadoPago;
using System.Text.Json;

namespace PaymentSystem.WebApi.MercadoPagoServices;

public class MerchantOrderService : MercadoPagoService, IMerchantOrderService
{
    private readonly string GET_PAYMENT_URL;

    public MerchantOrderService(IConfiguration configuration) : base(configuration)
    {
        GET_PAYMENT_URL = configuration["MercadoPagoUrls:GetPayment"] ??
                throw new Exception("There is no Url to Get Payment in appsettings.json");
    }

    public async Task<MerchantOrderDto?> GetMerchantOrderAsync(string merchantOrderUrl)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(merchantOrderUrl);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MerchantOrderDto>(jsonResponse);
    }

    public async Task<PaymentInfoDto?> GetMerchantOrderPaymentAsync(string paymentId, string token)
    {
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
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
