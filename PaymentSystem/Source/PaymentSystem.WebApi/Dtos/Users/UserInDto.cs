using PaymentSystem.WebApi.Enums;

namespace PaymentSystem.WebApi.Dtos.Users;

public class UserInDto
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserType UserType { get; set; }
    public string CompanyId { get; set; } = null!;
}
