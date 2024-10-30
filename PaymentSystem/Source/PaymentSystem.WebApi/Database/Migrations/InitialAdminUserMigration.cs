using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Extensions.Migration;
using PaymentSystem.WebApi.Entities;
using PaymentSystem.WebApi.Enums;

namespace PaymentSystem.WebApi.Database.Migrations;

public class InitialAdminUserMigration : IMigration
{
    public int Version => 1;
    private readonly IMongoCollection<Company> _companies;
    private readonly IMongoCollection<User> _users;

    public InitialAdminUserMigration(IMongoDatabase db)
    {
        _companies = db.GetCollection<Company>("companies");
        _users = db.GetCollection<User>("users");
        Up();
    }

    public void Down(BsonDocument document)
    {
        throw new NotImplementedException();
    }

    public void Up()
    {
        if (!_companies.Find(c => c.Name == "Administration Company").Any())
        {
            var company = new Company
            {
                Name = "Administration Company",
                Cnpj = string.Empty,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "InitialDataCreation"
            };
            _companies.InsertOne(company);
        }

        if (!_users.Find(u => u.Username == "admin").Any())
        {
            var insertedCompany = _companies.Find(c => c.Name == "Administration Company").ToList()[0];

            var user = new User
            {
                CompanyId = insertedCompany.Id,
                Name = "Admin",
                Username = "admin",
                PasswordHash = [76, 69, 150, 216, 232, 12, 68, 22, 44, 28, 133, 70, 10, 51, 237, 117, 253, 62, 14, 98, 12, 1, 152, 60, 245, 42, 160, 55, 44, 235, 83, 130, 5, 197, 133, 173, 185, 88, 232, 104, 4, 113, 211, 143, 167, 211, 242, 199, 212, 200, 111, 42, 208, 115, 206, 242, 199, 197, 51, 92, 239, 171, 159, 153],
                PasswordSalt = [205, 113, 28, 129, 117, 233, 143, 61, 60, 52, 189, 149, 225, 3, 237, 194, 223, 113, 123, 184, 178, 131, 65, 16, 225, 69, 31, 101, 142, 81, 37, 149, 179, 56, 154, 3, 130, 126, 61, 103, 141, 231, 144, 119, 16, 203, 40, 16, 189, 12, 66, 243, 35, 82, 83, 49, 20, 214, 61, 181, 23, 101, 28, 206, 42, 16, 208, 118, 195, 60, 141, 57, 111, 255, 85, 15, 107, 19, 177, 47, 34, 123, 44, 168, 212, 16, 198, 149, 169, 102, 107, 75, 222, 114, 39, 20, 92, 169, 83, 135, 253, 120, 178, 115, 140, 245, 16, 130, 35, 199, 156, 227, 42, 249, 77, 230, 90, 232, 60, 29, 134, 246, 161, 49, 206, 61, 199, 240],
                Email = "admin@test.com",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "InitialDataCreation",
                UserType = Enum.GetName(typeof(UserType), UserType.SystemAdmin) ?? "SystemAdmin"
            };
            _users.InsertOne(user);
        }
    }

    public void Up(BsonDocument document)
    {
        throw new NotImplementedException();
    }
}
