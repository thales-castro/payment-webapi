using MongoDB.Driver;
using PaymentWebApi.Entities;

namespace PaymentWebApi.Database.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory)
    {
        CollectionName = "orders";
    }

    public Task CreateNewDefaultOrderAsync(string mac_address, out Order order)
    {
        Order default_order = Order.LoadDefault(mac_address);
        order = default_order;
        return CreateAsync(default_order);
    }

    public Task<Order> GetLastOrder(string mac_address)
    {
        return GetCollection()
            .Find(o => o.mac_address == mac_address)
            .SortByDescending(o => o.CreatedAt)
            .FirstOrDefaultAsync();
    }

    public Task<Order> GetOrderByExternalReferenceAsync(string externalReference)
    {
        return GetCollection()
            .Find(o => o.external_reference == externalReference)
            .FirstOrDefaultAsync();
    }
}