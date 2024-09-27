using MongoDB.Driver;
using PaymentWebApi.Entities;

namespace PaymentWebApi.Database.Repositories;

public class MerchantOrderRepository : GenericRepository<MerchantOrder>, IMerchantOrderRepository
{
    public MerchantOrderRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory)
    {
        CollectionName = "merchant_order";
    }
}
