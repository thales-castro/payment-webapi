using PaymentSystem.WebApi.Dtos.MercadoPago;
using PaymentSystem.WebApi.Entities;
using PaymentSystem.WebApi.Entities.MercadoPagoEntities;
using System.Text.Json;

namespace PaymentSystem.WebApi.MercadoPagoServices
{
    public class OrderService : MercadoPagoService, IOrderService
    {
        public OrderService(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Order?> GetCurrentOrderAsync(long user_id, string cashier_external_id, string token)
        {
            SetBearerToken(token);

            var mpUrl = _configuration["MercadoPagoUrls:GetOrder"] ??
                throw new Exception("There is no Url to Get Order in appsettings.json");
            string str_address = string.Format(mpUrl, user_id, cashier_external_id);
            var response = await _httpClient.GetAsync(str_address);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            Error? error = JsonSerializer.Deserialize<Error>(jsonResponse);
            if (error != null && error.status == 404)
                return null;

            return JsonSerializer.Deserialize<Order>(jsonResponse);
        }

        public async Task<bool> CreateNewOrderAsync(long user_id, string store_external_id,
            string cashier_external_id, string token, OrderDto order)
        {
            SetBearerToken(token);

            var mpUrl = _configuration["MercadoPagoUrls:CreateOrder"] ??
                throw new Exception("There is no Url to Create Order in appsettings.json");
            string str_address = string.Format(mpUrl, user_id, store_external_id, cashier_external_id);
            var response = await _httpClient.PutAsJsonAsync(str_address, order);
            if ((int)response.StatusCode != StatusCodes.Status204NoContent)
                throw new Exception($"Error on creating Order in MP call: {await response.Content.ReadAsStringAsync()}");

            return true;
        }

        public async Task<bool> CheckIfOrderIsPaidAsync(string external_reference)
        {
            var mpUrl = _configuration["MercadoPagoUrls:CheckOrderPayment"] ??
                throw new Exception("There is no Url to Check if Order is Paid in appsettings.json");
            string str_address = string.Format(mpUrl, external_reference);
            HttpResponseMessage response = await _httpClient.GetAsync(str_address);
            string sJsonContent = await response.Content.ReadAsStringAsync();
            if (sJsonContent != null)
            {
                Dictionary<string, object>? jsonResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(sJsonContent);
                if (jsonResponse == null || jsonResponse["results"] == null)
                    return false;
                string? sResults = jsonResponse["results"].ToString();
                if (sResults != null)
                {
                    Dictionary<string, object>[]? kvResults = JsonSerializer.Deserialize<Dictionary<string, object>[]>(sResults);
                    if (kvResults == null)
                        return false;
                    return kvResults.Length > 0;
                }
            }
            return false;
        }
    }
}
