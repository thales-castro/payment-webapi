namespace PaymentSystem.WebApi.Dtos;

public class PaymentFilterDto
{
    public string? CompanyId { get; set; }
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public int Page
    { 
        get => _page; 
        init => _page = value >= 1 ? value + 1 : 1; 
    }
    public int Size 
    {
        get => _size; 
        init => _size = value >= 1 ? GetMin(value,100) : 10; 
    }

    private readonly int _page = 1;
    private readonly int _size = 10;

    private int GetMin(params int[] values) => values.Min();
}
