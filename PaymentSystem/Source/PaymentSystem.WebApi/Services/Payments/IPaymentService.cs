using PaymentSystem.WebApi.Dtos;

namespace PaymentSystem.WebApi.Services.Payments;

public interface IPaymentService
{
     Task<List<PaymentDto>> GetPayments();
}
