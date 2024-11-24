using PaymentSystem.WebApi.Dtos;

namespace PaymentSystem.WebApi.Services.Payments;

public interface IPaymentService
{
     Task<PaymentDtoList> GetPayments(PaymentFilterDto paymentFilter);
}
