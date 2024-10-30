namespace PaymentSystem.WebApi.Services.Auth;

public interface IAuthService
{
    Task<string> Login(string username, string password);
}
