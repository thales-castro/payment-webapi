using MongoDB.Driver;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public class MerchantOrderPaymentRepository : GenericRepository<MerchantOrderPayment>, IMerchantOrderPaymentRepository
{
    public MerchantOrderPaymentRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory)
    {
        CollectionName = "merchant_order_payment";
    }
}
