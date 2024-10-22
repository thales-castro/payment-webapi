using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public interface IPaymentDeviceRepository : IGenericRepository<PaymentDevice>
{
    Task<PaymentDevice> GetDeviceByMacAddressAsync(string macAddress);

    Task<PaymentDevice> GetDeviceByUserId(long userId);
}
