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

    public async Task<PaymentDtoList> GetPayments(PaymentFilterDto paymentFilter)
    {
        List<PaymentDto> result = new List<PaymentDto>();

        List<Order> orders;

        if (paymentFilter.StartDate != null && 
           paymentFilter.StartDate != string.Empty &&
           paymentFilter.EndDate != null &&
           paymentFilter.EndDate != string.Empty)
        {
            var startDateFilter = DateTime.Parse(paymentFilter.StartDate);
            var endDateFilter = DateTime.Parse(paymentFilter.EndDate);
            orders = _orderRepository.ReadAll().Where(o => !o.IsRemoved &&
                    o.CreatedAt >= startDateFilter && o.CreatedAt <= endDateFilter).ToList();
        }
        else
        {
            orders = _orderRepository.ReadAll().Where(o => !o.IsRemoved).ToList();
        }

        double total = 0;
        foreach(Order o in orders)
        {
            PaymentDevice device = await _paymentDeviceRepository.GetDeviceByMacAddressAsync(o.mac_address);
            if(device != null)
            {
                Company company = await _companyRepository.GetByIdAsync(device.CompanyId);
                if (paymentFilter.CompanyId != null && paymentFilter.CompanyId != string.Empty)
                {
                    if (company != null && company.Id != paymentFilter.CompanyId) 
                    { 
                        continue;
                    }
                }
                if (company != null && device.MacAddress != null && company.Name != null)
                {
                    PaymentDto dto = new PaymentDto(o.Id, device.MacAddress,
                        company.Name, o.status, Order.GetDescription(o.status),
                        o.items[0].title, o.items[0].unit_price, o.CreatedAt.ToString(), 
                        o.UpdatedAt == null?"" : o.UpdatedAt.ToString());
                    total += dto.Value;
                    result.Add(dto);
                }
            }
        }

        return new PaymentDtoList
        {
            Total = total,
            Items = result
        };
    }
}
