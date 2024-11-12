namespace PaymentSystem.WebApi.Dtos.Companies;

public class CompanyDto
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string? MpUserId { get; set; }
    public int? MpStoreInternalId { get; set; }
    public string? MpStoreExternalReference { get; set; }
    public string? Token { get; set; }
}
