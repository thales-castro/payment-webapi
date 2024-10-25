using MongoDB.Bson.Serialization.Attributes;

namespace PaymentSystem.WebApi.Entities;

public class MerchantOrderPayment : BaseEntity
{
    [BsonElement("mp_id")]
    public long mp_id { get; set; }
    [BsonElement("order_id")]
    public string order_id { get; set; }
    [BsonElement("external_reference")]
    public string external_reference { get; set; }
    [BsonElement("merchant_order_id")]
    public string merchant_order_id { get; set; }
    [BsonElement("status")]
    public string status { get; set; }

    public MerchantOrderPayment(long mp_id, string order_id, string external_reference, string merchant_order_id, string status)
    {
        this.mp_id = mp_id;
        this.order_id = order_id;
        this.external_reference = external_reference;
        this.merchant_order_id = merchant_order_id;
        this.status = status;
    }
}
