using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task CreateNewDefaultOrderAsync(string mac_address, out Order order);

    Task<Order> GetLastOrder(string mac_address);

    Task<Order> GetOrderByExternalReferenceAsync(string externalReference);
}

