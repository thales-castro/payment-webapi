using MongoDB.Driver;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public class PaymentDeviceRepository : GenericRepository<PaymentDevice>, IPaymentDeviceRepository
{
    public PaymentDeviceRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory) =>
        CollectionName = "payment_devices";

    public Task<PaymentDevice> GetDeviceByMacAddressAsync(string macAddress) =>
        GetCollection().Find(x => x.MacAddress == macAddress).FirstOrDefaultAsync();

    public async Task<PaymentDevice?> GetByIdAsync(string id) =>
        await GetCollection().Find(doc => doc.Id == id).FirstOrDefaultAsync();

    public async Task<List<PaymentDevice>> GetNotDeletedAsync() =>
        await GetCollection().Find(doc => !doc.IsRemoved).ToListAsync();

    public async Task<bool> CheckIfMacExistsAsync(string macAddress) =>
        await GetCollection().CountDocumentsAsync(doc => doc.MacAddress == macAddress) > 0;

    public async Task<bool> CheckIfCashierInternalMPIdExistsAsync(string cashierInternalMPId) =>
        await GetCollection().CountDocumentsAsync(doc => doc.CashierInternalMPId == cashierInternalMPId) > 0;
}
