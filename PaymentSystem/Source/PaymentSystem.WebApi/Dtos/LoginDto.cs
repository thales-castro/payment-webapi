namespace PaymentSystem.WebApi.Dtos;

/// <summary>
/// DTO Used to login
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = string.Empty;
    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
