using Microsoft.AspNetCore.Mvc;
using PaymentWebApi.MercadoPago;
using PaymentWebApi.PaymentEntities;
using System.Text.Json;

namespace PaymentWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AfePaymentController : ControllerBase
    {
        private readonly ILogger<AfePaymentController> _logger;
        private readonly OrderService _orderService;
        private readonly MerchantOderService _merchantOderService;
     
        public AfePaymentController(ILogger<AfePaymentController> logger)
        {
            _logger = logger;
            _orderService = new();
            _merchantOderService = new();
        }

        [HttpGet]
        public async Task<bool> GetPaymentState(int user_id, string store_external_id, string cashier_id)
        {
            //Existe uma ordem corrent.
            //Verificar se a mesma foi paga, será que se paga some antes
            bool ret_val = _orderService.OrderIsPaid("12345");

            Order? current_order =  await _orderService.GetCurrentOrderAsync(user_id, cashier_id);

            if (current_order == null)
            {
                //Se a ordem existem no banco limpa e cria de novo.
                bool was_created = await _orderService.CreateNewOrderAsync(user_id, store_external_id, cashier_id);
            }
                       
            return ret_val;

        }

        [HttpPost]
        public async Task<IActionResult> NotifyPayment([FromBody] JsonElement notify_data)
        {
            try
            {
                JsonElement topic_value = default(JsonElement);
                if (notify_data.TryGetProperty("topic", out topic_value))
                {
                    TopicMessage? topic_message =
                        JsonSerializer.Deserialize<TopicMessage>(notify_data);
                    if (topic_message != null)
                    {
                        MerchantOrder? merchant_order =
                             await _merchantOderService.GetOrder(topic_message.resource);

                        if (topic_message.topic == "merchant_order" && merchant_order != null)
                        {
                            _logger.LogInformation("An payment has started");
                            if (merchant_order != null)
                                _orderService.InserMerchantOrder(merchant_order);
                        }
                        else if (topic_message.topic == "payment" && merchant_order != null)
                        {
                            OrderPaymentNotification? orderPaymentNotification = 
                                await _merchantOderService.GetOrderPaymentNotificationAsync(topic_message.resource);
                            if (orderPaymentNotification != null)
                            {
                                _orderService.SetMerchantOrderPaid(orderPaymentNotification.external_reference);
                            }                            
                        }
                    }
                }
            } catch 
            { 
            }
            
            /*
            else
            {
                PaymentNotification? payment_notification =
                    JsonSerializer.Deserialize<PaymentNotification>(notify_data);
                if(payment_notification != null)
                    _orderService.SetMerchantOrderPaid(payment_notification.id);
                _logger.LogInformation(payment_notification?.ToString());
            }
            */

            return Ok();
        }
    }
}
