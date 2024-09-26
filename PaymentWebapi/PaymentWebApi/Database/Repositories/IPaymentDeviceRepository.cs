using PaymentWebApi.Entities;

namespace PaymentWebApi.Database.Repositories;

public interface IPaymentDeviceRepository : IGenericRepository<PaymentDevice>
{
    Task<PaymentDevice> GetDeviceByMacAddressAsync(string macAddress);
}
