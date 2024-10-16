using MongoDB.Bson.Serialization.Attributes;

namespace PaymentWebApi.Entities
{
    public class MerchantOrder : BaseEntity
    {
        [BsonElement("mp_id")]
        public long mp_id { get; set; }
        [BsonElement("status")]
        public string status { get; set; }
        [BsonElement("external_reference")]
        public string external_reference { get; set; }
        [BsonElement("cancelled")]
        public bool cancelled { get; set; }
        [BsonElement("order_status")]
        public string order_status { get; set; }

        public MerchantOrder(long mp_id, string status, string external_reference, bool cancelled, string order_status)
        {
            this.mp_id = mp_id;
            this.status = status;
            this.external_reference = external_reference;
            this.cancelled = cancelled;
            this.order_status = order_status;
        }
    }
}
