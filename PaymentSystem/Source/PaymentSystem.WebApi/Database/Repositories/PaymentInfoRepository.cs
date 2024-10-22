using MongoDB.Driver;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public class PaymentInfoRepository : GenericRepository<PaymentInfo>, IPaymentInfoRepository
{
    protected PaymentInfoRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory)
    {
        CollectionName = "payment_infos";
    }
}
