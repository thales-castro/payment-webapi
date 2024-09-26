using PaymentWebApi.Dtos.MercadoPago;
using System.Text.Json;

namespace PaymentWebApi.MercadoPagoServices;

public class MerchantOrderService : MercadoPagoService, IMerchantOrderService
{
    public async Task<MerchantOrderDto?> GetMerchantOrderAsync(string merchantOrderUrl)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(merchantOrderUrl);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MerchantOrderDto>(jsonResponse);
    }

    public async Task<MerchantOrderPaymentDto?> GetMerchantOrderPaymentAsync(string merchantOrderPaymentUrl)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(merchantOrderPaymentUrl);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        if (jsonResponse != null)
        {
            Dictionary<string, Object>? responseDict = JsonSerializer.Deserialize<Dictionary<string, Object>>(jsonResponse);
            if (responseDict != null && responseDict["collection"] != null)
            {
                return JsonSerializer.Deserialize<MerchantOrderPaymentDto?>(responseDict["collection"].ToString());
            }
        }
        return null;
    }
}
