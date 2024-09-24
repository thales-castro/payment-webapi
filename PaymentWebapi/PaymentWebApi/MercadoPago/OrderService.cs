using PaymentWebApi.Dao;
using PaymentWebApi.PaymentEntities;

namespace PaymentWebApi.MercadoPago
{
    public class OrderService
    {
        //TODO: Put it in the configuration.
        private readonly string _token = "APP_USR-6099630597304134-092008-9b1e83a93622d940a234083eeeee63bd-1994736709";
        private HttpClient _httpClient;
        private OrderDao _order_dao;
        private Order _default_order;

        public OrderService()
        {
            _order_dao = new OrderDao();
            _default_order = _order_dao.LoadDefault();
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
        }

        public async Task<Order?> GetCurrentOrderAsync(int store_id, string cashier_id)
        {
            //TODO: Move hard coded address to config.
            string str_address = $"https://api.mercadopago.com/instore/qr/seller/collectors/{store_id}/pos/{cashier_id}/orders";                      
            HttpResponseMessage response = await _httpClient.GetAsync(str_address);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            Error? error = System.Text.Json.JsonSerializer.Deserialize<Error>(jsonResponse);
            if (error != null && error.status == 404)
                return null;

            return System.Text.Json.JsonSerializer.Deserialize<Order>(jsonResponse);
        }

        public async Task<bool> CreateNewOrderAsync(int store_id, string store_external_id, string cashier_id)
        {
            //TODO: Move hard coded address to config.
            string str_address = $"https://api.mercadopago.com/instore/qr/seller/collectors/{store_id}/stores/{store_external_id}/pos/{cashier_id}/orders";
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<Order>(str_address, _default_order);
            _order_dao.InsertOrder(_default_order);
            return true;
        }

        public bool OrderIsPaid(string external_reference)
        {
            return _order_dao.OrderIsPaid(external_reference);
        }

        public void InserMerchantOrder(MerchantOrder merchant_order)
        {
            _order_dao.InsertMerchantOrder(merchant_order);
        }

        public void SetMerchantOrderPaid(string external_reference)
        {
            _order_dao.SetMerchantPaid(external_reference);
        }

    }
}
