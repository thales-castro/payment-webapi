namespace PaymentWebApi.Entities
{
    public class MerchantOrder : BaseEntity
    {
        public string status { get; set; }
        public string external_reference { get; set; }
        public bool cancelled { get; set; }
        public string order_status { get; set; }

        public MerchantOrder(string id, string status, string external_reference, bool cancelled, string order_status)
        {
            this.Id = id;
            this.status = status;
            this.external_reference = external_reference;
            this.cancelled = cancelled;
            this.order_status = order_status;
        }
    }
}
