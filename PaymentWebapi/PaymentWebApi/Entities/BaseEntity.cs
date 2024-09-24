using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PaymentWebApi.Entities;

public class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; set; } //TODO: Verify need to be protected
    [BsonElement("UpdatedAt")]
    public DateTime? UpdatedAt { get; set; }
    [BsonElement("RemovedAt")]
    public DateTime? RemovedAt { get; set; }
    [BsonElement("CreatedBy")]
    public string CreatedBy { get; set; } = string.Empty;
    [BsonElement("UpdatedBy")]
    public string? UpdatedBy { get; set; }
    [BsonElement("RemovedBy")]
    public string RemovedBy { get; set; } = string.Empty;
    [BsonElement("IsRemoved")]
    public bool IsRemoved { get; set; }
}

