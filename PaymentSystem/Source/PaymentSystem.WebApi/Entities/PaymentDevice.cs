using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Extensions.Migration;

namespace PaymentSystem.WebApi.Entities;

public class PaymentDevice : BaseEntity, IVersioned
{
    // O Id da entidade no mongo também é o Id interno do Caixa no MP.
    [BsonElement("CompanyId")]
    public string CompanyId { get; set; } = null!;
    [BsonElement("MacAddress")]
    public string MacAddress { get; set; } = string.Empty;
    [BsonElement("CashierInternalMPId")]
    public string CashierInternalMPId { get; set; } = string.Empty;
    public int Version { get; set; }
}
