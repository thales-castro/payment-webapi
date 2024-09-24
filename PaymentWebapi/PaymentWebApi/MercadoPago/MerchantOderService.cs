using PaymentWebApi.Dao;
using PaymentWebApi.PaymentEntities;
using System.Text.Json;

namespace PaymentWebApi.MercadoPago
{
    public class MerchantOderService
    {
        //TODO: Put it in the configuration.
        //TODO: Create inheritance
        private readonly string _token = "APP_USR-6099630597304134-092008-9b1e83a93622d940a234083eeeee63bd-1994736709";
        private HttpClient _httpClient;

        public MerchantOderService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
        }

        public async Task<MerchantOrder?> GetOrder(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<MerchantOrder>(jsonResponse);
        }

        public async Task<OrderPaymentNotification?> GetOrderPaymentNotificationAsync(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Dictionary<string, object>? dic_response = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);
            Dictionary<string, object>? dic_payment = JsonSerializer.Deserialize<Dictionary<string, object>>(dic_response["collection"].ToString());

            OrderPaymentNotification paymentNotification = new OrderPaymentNotification(dic_payment["order_id"].ToString(), dic_payment["external_reference"].ToString());

            return paymentNotification;

        }
    }
}
