namespace PaymentSystem.WebApi.Dtos;

public class PaymentDeviceDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string CompanyId { get; set; } = null!;
    public string MacAddress { get; set; } = null!;
    public string CashierInternalMPId { get; set; } = null!;
    public string SellItemDescr { get; set; } = null!;  
    public string SellItemValue { get; set; } = null!;

}
