using MongoDB.Driver;
using PaymentWebApi.Entities;

namespace PaymentWebApi.Database.Repositories;

public class MerchantOrderPaymentRepository : GenericRepository<MerchantOrderPayment>, IMerchantOrderPaymentRepository
{
    public MerchantOrderPaymentRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory)
    {
        CollectionName = "merchant_order_payment";
    }
}
