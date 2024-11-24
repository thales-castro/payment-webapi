

using System.Text;
using System.Text.Json;

namespace PaymentSystem.WebApi.MercadoPagoServices;

public class CashierService : MercadoPagoService, ICashierService
{
    public CashierService(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task SetExternalIdAsync(string storeExternalCashierIdToSet, string cashierName, string mpInternalCashierId, string token)
    {
        SetBearerToken(token);

        var mpUrl = _configuration["MercadoPagoUrls:UpdateCashier"] ??
                throw new Exception("There is no Url to Update Cashier in appsettings.json");

        var strAddress = string.Format(mpUrl, mpInternalCashierId);
        var body = new { name = cashierName, external_id = storeExternalCashierIdToSet };
        string bodyStr = JsonSerializer.Serialize(body);
        var strContent = new StringContent(bodyStr, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(strAddress, strContent);

        if ((int)response.StatusCode != StatusCodes.Status200OK)
            throw new Exception($"Erro ao salvar o Id Externo do Caixa do Mercado Pago: [{await response.Content.ReadAsStringAsync()}]");
    }
}
