using MongoDB.Driver;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public class MerchantOrderRepository : GenericRepository<MerchantOrder>, IMerchantOrderRepository
{
    public MerchantOrderRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory)
    {
        CollectionName = "merchant_order";
    }
}
