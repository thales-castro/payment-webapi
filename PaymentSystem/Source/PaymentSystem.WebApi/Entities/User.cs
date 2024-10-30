using MongoDB.Bson.Serialization.Attributes;

namespace PaymentSystem.WebApi.Entities;

public class User : BaseEntity
{
    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;
    [BsonElement("Username")]
    public string Username { get; set; } = string.Empty;
    [BsonElement("PasswordHash")]
    public byte[]? PasswordHash { get; set; }
    [BsonElement("PasswordSalt")]
    public byte[]? PasswordSalt { get; set; }
    [BsonElement("UserType")]
    public string UserType { get; set; } = string.Empty;
    [BsonElement("CompanyId")]
    public string CompanyId { get; set; } = string.Empty;
    [BsonElement("Email")]
    public string Email { get; set; } = string.Empty;
}
