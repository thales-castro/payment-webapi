using MongoDB.Bson.Serialization.Attributes;

namespace PaymentWebApi.Entities;

public class PaymentInfo : BaseEntity
{
    [BsonElement("date_approved")]
    public string? date_approved { get; set; }
    [BsonElement("date_created")]
    public string? date_created { get; set; }
    [BsonElement("date_last_updated")]
    public string? date_last_updated { get; set; }
    [BsonElement("description")]
    public string? description { get; set; }
    [BsonElement("external_reference")]
    public string? external_reference { get; set; }
    [BsonElement("money_release_date")]
    public string? money_release_date { get; set; }
    [BsonElement("notification_url")]
    public string? notification_url { get; set; }
    [BsonElement("status")]
    public string? status { get; set; }
    [BsonElement("store_id")]
    public string? store_id { get; set; }
    [BsonElement("payment_order_id")]
    public string? payment_order_id { get; set; }
    [BsonElement("payment_order_type")]
    public string? payment_order_type { get; set; }
}
