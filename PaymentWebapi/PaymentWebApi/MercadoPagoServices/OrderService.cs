using PaymentWebApi.Dtos.MercadoPago;
using PaymentWebApi.Entities;
using PaymentWebApi.Entities.MercadoPagoEntities;
using PaymentWebApi.MercadoPagoServices;
using System.Text.Json;

namespace PaymentWebApi.MercadoPago
{
    public class OrderService : MercadoPagoService, IOrderService
    {
     
        public OrderService() : base()
        {
        }

        public async Task<Order?> GetCurrentOrderAsync(long user_id, string cashier_external_id, string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            string str_address = $"https://api.mercadopago.com/instore/qr/seller/collectors/{user_id}/pos/{cashier_external_id}/orders";                      
            HttpResponseMessage response = await _httpClient.GetAsync(str_address);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Error? error = JsonSerializer.Deserialize<Error>(jsonResponse);
            if (error != null && error.status == 404)
                return null;
            return JsonSerializer.Deserialize<Order>(jsonResponse);
        }

        public async Task<bool> CreateNewOrderAsync(long user_id, string store_external_id, 
            string cashier_external_id, string token, OrderDto order)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            //TODO: Move hard coded address to config.
            string str_address = $"https://api.mercadopago.com/instore/qr/seller/collectors/{user_id}/stores/{store_external_id}/pos/{cashier_external_id}/orders";
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<OrderDto>(str_address, order);
            return true;
        }

        public async Task<bool> CheckIfOrderPaidAsync(string external_reference)
        {
            string str_address = $"https://api.mercadopago.com/v1/payments/search?external_reference={external_reference}";
            HttpResponseMessage response = await _httpClient.GetAsync(str_address);
            string sJsonContent = await response.Content.ReadAsStringAsync();
            if (sJsonContent != null)
            {
                Dictionary<string, object>? jsonResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(sJsonContent);
                if (jsonResponse == null || jsonResponse["results"] == null)
                    return false;
                Dictionary<string, object>[]? kvResults = JsonSerializer.Deserialize<Dictionary<string, object>[]>(jsonResponse["results"].ToString());
                if (kvResults == null)
                    return false;
                return kvResults.Length > 0;
            }
            return false;
        }
    }
}
