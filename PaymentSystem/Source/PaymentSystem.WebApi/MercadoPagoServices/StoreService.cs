using System.Text;
using System.Text.Json;

namespace PaymentSystem.WebApi.MercadoPagoServices;

public class StoreService : MercadoPagoService, IStoreService
{
    public StoreService(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task SetExternalIdAsync(string mpInternalUserId, int mpInternalStoreId, string token, string storeExternalIdToSet)
    {
        SetBearerToken(token);

        var mpUrl = _configuration["MercadoPagoUrls:UpdateStore"] ??
                throw new Exception("There is no Url to Update Store in appsettings.json");

        var strAddress = string.Format(mpUrl, mpInternalUserId, mpInternalStoreId);
        var body = new { external_id = storeExternalIdToSet };
        string bodyStr = JsonSerializer.Serialize(body);
        var strContent = new StringContent(bodyStr, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(strAddress, strContent);

        if ((int)response.StatusCode != StatusCodes.Status200OK)
            throw new Exception($"Erro ao salvar o Id Externo da Loja do Mercado Pago: [{await response.Content.ReadAsStringAsync()}]");
    }
}
