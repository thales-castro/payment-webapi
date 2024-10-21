using MongoDB.Driver;
using PaymentWebApi.Entities;

namespace PaymentWebApi.Database.Repositories;

public class PaymentInfoRepository : GenericRepository<PaymentInfo>, IPaymentInfoRepository
{
    protected PaymentInfoRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory)
    {
        CollectionName = "payment_infos";
    }
}
