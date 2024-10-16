using MongoDB.Driver;
using PaymentWebApi.Entities;

namespace PaymentWebApi.Database.Repositories;

public class PaymentDeviceRepository : GenericRepository<PaymentDevice>, IPaymentDeviceRepository
{
    public PaymentDeviceRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory)
    {
        CollectionName = "payment_devices";
    }

    public Task<PaymentDevice> GetDeviceByMacAddressAsync(string macAddress)
    {
        return GetCollection().Find(x => x.MacAddress == macAddress).FirstOrDefaultAsync();
    }

    public Task<PaymentDevice> GetDeviceByUserId(long userId)
    {
        return GetCollection().Find(x => x.UserId == userId).FirstOrDefaultAsync();
    }
}
