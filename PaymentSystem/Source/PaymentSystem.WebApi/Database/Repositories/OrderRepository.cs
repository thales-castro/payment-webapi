using MongoDB.Driver;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

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

    public Task CreateOrderAsync(string mac_address, string itemDescr, double itemValue, out Order newOrder)
    {
        Order order = Order.CreateNewOrder(mac_address, itemDescr, itemValue);
        newOrder = order;
        return CreateAsync(order);
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