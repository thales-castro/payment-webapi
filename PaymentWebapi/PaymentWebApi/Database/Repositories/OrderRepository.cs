using MongoDB.Driver;
using PaymentWebApi.PaymentEntities;

namespace PaymentWebApi.Database.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory)
    {
        CollectionName = "orders";
    }
}