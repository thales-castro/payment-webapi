using Microsoft.IdentityModel.Tokens;
using PaymentSystem.WebApi.Entities;
using PaymentSystem.WebApi.Exceptions;
using PaymentSystem.WebApi.Extensions;
using PaymentSystem.WebApi.Services.Companies;
using PaymentSystem.WebApi.Services.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PaymentSystem.WebApi.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly ICompanyService _companyService;
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration, IUserService userService, ICompanyService companyService)
    {
        _configuration = configuration;
        _userService = userService;
        _companyService = companyService;
    }

    public async Task<string> Login(string username, string password)
    {
        var user = await _userService.GetByUsernameAsync(username);

        if (user == null)
            throw new EntityNotFoundException("Username not found.");

        if (!CriptographyExtension.CheckPasswordHash(password, user.PasswordHash!, user.PasswordSalt!))
            throw new BadRequestException("Password is wrong.");
        return await CreateJwtToken(user);
    }

    private async Task<string> CreateJwtToken(User user)
    {
        var userCompanyName = await _companyService.GetNameByIdAsync(user.CompanyId);

        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Name),
            new ("Username", user.Username),
            new ("Email", user.Email),
            new ("UserType", user.UserType),
            new ("CompanyId", user.CompanyId.ToString() ?? string.Empty),
            new ("CompanyName", userCompanyName)
        };

        var secret = _configuration.GetSection("Security:Secret").Value ?? 
            "eP@9mY2r3L#7xZ6tVq9$Gf1jU8yKq5mBzL*4rY0nJ&8sH3zW@r2T^6kV#4tM&1pQ!3cF7wD*9sXjB1$yN*8eZ^5gR#2hA%3kT^0jQ!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
