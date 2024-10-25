using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Extensions.Migration;

namespace PaymentSystem.WebApi.Entities;

public class PaymentDevice : BaseEntity, IVersioned
{
    [BsonElement("MacAddress")]
    public string? MacAddress { get; set; }
    [BsonElement("UserId")]
    public long UserId { get; set; }
    [BsonElement("StoreExternalId")]
    public string? StoreExternalId { get; set; }
    [BsonElement("CashierExternalId")]
    public string? CashierExternalId { get; set; }
    public string? Token { get; set; }
    public int Version { get; set; }
}
