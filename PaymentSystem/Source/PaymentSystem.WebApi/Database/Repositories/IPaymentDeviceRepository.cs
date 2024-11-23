using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public interface IPaymentDeviceRepository : IGenericRepository<PaymentDevice>
{
    Task<PaymentDevice> GetDeviceByMacAddressAsync(string macAddress);
    Task<PaymentDevice?> GetByIdAsync(string id);
    Task<List<PaymentDevice>> GetNotDeletedAsync();
    Task<bool> CheckIfMacExistsAsync(string macAddress);
    Task<bool> CheckIfCashierInternalMPIdExistsAsync(string cashierInternalMPId);
}
