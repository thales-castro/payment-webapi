namespace PaymentSystem.WebApi.ViewModels;

public class PaymentDeviceViewModel
{
    public string Id { get; set; } = null!;
    public string CompanyId { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string MacAddress { get; set; } = null!;
    public string CashierExternalId { get; set; } = null!;
    public string CashierInternalMPId { get; set; } = null!;
    public string SellItemDescr { get; set; } = null!;
    public string SellItemValue { get; set; } = null!;
}
