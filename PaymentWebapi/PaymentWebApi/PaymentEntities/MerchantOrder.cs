namespace PaymentWebApi.PaymentEntities
{
    public class MerchantOrder
    {
        public long id { get; set; }
        public string status { get; set; }
        public string external_reference { get; set; }

        public MerchantOrder(long id, string status, string external_reference)
        {
            this.id = id;
            this.status = status;
            this.external_reference = external_reference;
        }
    }
}
