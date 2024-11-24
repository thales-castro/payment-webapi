namespace PaymentSystem.WebApi.Dtos;

public class PaymentFilterDto
{
    public string CompanyId { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
}
