using PaymentSystem.WebApi.Database.Repositories;
using PaymentSystem.WebApi.Dtos;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Services.Payments;

public class PaymentService : IPaymentService
{
    private IOrderRepository _orderRepository;
    private IPaymentDeviceRepository _paymentDeviceRepository;
    private ICompanyRepository _companyRepository;

    public PaymentService(IOrderRepository orderRepository, 
        IPaymentDeviceRepository paymentDeviceRepository,
        ICompanyRepository companyRepository) 
    { 
        _orderRepository = orderRepository;
        _paymentDeviceRepository = paymentDeviceRepository;
        _companyRepository = companyRepository;
    }

    public async Task<List<PaymentDto>> GetPayments()
    {
        List<PaymentDto> result = new List<PaymentDto>();
        List<Order> orders = _orderRepository.ReadAll().Where(o => !o.IsRemoved).ToList();
        foreach(Order o in orders)
        {
            PaymentDevice device = await _paymentDeviceRepository.GetDeviceByMacAddressAsync(o.mac_address);
            if(device != null)
            {
                Company company = await _companyRepository.GetByIdAsync(device.CompanyId);
                if (company != null && device.MacAddress != null && company.Name != null
                    && o.UpdatedAt != null)
                {
                    PaymentDto dto = new PaymentDto(o.Id, device.MacAddress,
                        company.Name, o.status, Order.GetDescription(o.status),
                        o.items[0].title, o.items[0].unit_price, o.CreatedAt.ToString(), 
                        o.UpdatedAt.ToString());
                    result.Add(dto);
                }
            }
        }
        return result;
    }
}
