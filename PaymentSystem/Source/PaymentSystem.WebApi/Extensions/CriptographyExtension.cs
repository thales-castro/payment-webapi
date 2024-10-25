using System.Security.Cryptography;
using System.Text;

namespace PaymentSystem.WebApi.Extensions;
public static class CriptographyExtension
{
    public static void GetPasswordHash(string passwordStr, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordStr));
    }

    public static bool CheckPasswordHash(string insertedPassword, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var insertedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(insertedPassword));
        return passwordHash.SequenceEqual(insertedHash);
    }
}