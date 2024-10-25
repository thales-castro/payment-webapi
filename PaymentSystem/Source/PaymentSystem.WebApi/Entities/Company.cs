using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Extensions.Migration;

namespace PaymentSystem.WebApi.Entities;

public class Company : BaseEntity, IVersioned
{
    [BsonElement("Name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("Cnpj")]
    public string Cnpj { get; set; } = string.Empty;
    public int Version { get; set; }
}
