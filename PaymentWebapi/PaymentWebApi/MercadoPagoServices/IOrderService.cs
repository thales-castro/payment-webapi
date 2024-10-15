using PaymentWebApi.Dtos.MercadoPago;
using PaymentWebApi.Entities;

namespace PaymentWebApi.MercadoPagoServices;

public interface IOrderService
{
    Task<Order?> GetCurrentOrderAsync(long user_id, string cashier_external_id, string token);
    Task<bool> CreateNewOrderAsync(long user_id, string store_external_id, 
        string cashier_external_id, string token, OrderDto order);
}
