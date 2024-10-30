namespace PaymentSystem.WebApi.Dtos.Users;

public class UserOutDto
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserType { get; set; } = string.Empty;
    public string CompanyId { get; set; } = null!;
    public bool IsRemoved { get; set; }
}
