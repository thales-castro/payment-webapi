using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Extensions.Migration;

namespace PaymentSystem.WebApi.Entities;

public class PaymentDevice : BaseEntity, IVersioned
{
    [BsonElement("CompanyId")]
    public string CompanyId { get; set; } = null!;
    [BsonElement("MacAddress")]
    public string? MacAddress { get; set; }
    [BsonElement("CashierExternalId")]
    public string? CashierExternalId { get; set; }
    public int Version { get; set; }
}
//[BsonElement("UserId")] // vai pra company
//public long UserId { get; set; } // vai pra company
//[BsonElement("StoreExternalId")] // vai pra company
//public string? StoreExternalId { get; set; }// vai pra comppany
//[BsonElement("StoreExternalId")] // vai pra comppany
//public string? Token { get; set; }// vai pra comppany
