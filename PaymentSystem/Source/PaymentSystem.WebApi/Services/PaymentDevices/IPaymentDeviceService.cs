using PaymentSystem.WebApi.Dtos;
using PaymentSystem.WebApi.ViewModels;

namespace PaymentSystem.WebApi.Services.PaymentDevices;

public interface IPaymentDeviceService
{
    PaymentDeviceDto Register(PaymentDeviceDto dto);
    List<PaymentDeviceDto> GetAll();
    Task<PaymentDeviceDto> GetByIdAsync(string id);
    Task<PaymentDeviceDto> UpdateAsync(PaymentDeviceDto dto);
    PaymentDeviceDto Delete(string id);
    Task<List<PaymentDeviceViewModel>> GetNotDeletedAsync();
}
