using PaymentSystem.WebApi.Dtos.Companies;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public interface IPaymentDeviceRepository : IGenericRepository<PaymentDevice>
{
    Task<PaymentDevice> GetDeviceByMacAddressAsync(string macAddress);
    Task<PaymentDevice?> GetByIdAsync(string id);
    Task<List<PaymentDevice>> GetNotDeletedAsync();
}
