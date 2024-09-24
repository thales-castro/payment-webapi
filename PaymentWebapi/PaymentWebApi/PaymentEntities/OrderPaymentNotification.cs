namespace PaymentWebApi.PaymentEntities
{
    
    public class OrderPaymentNotification
    {
        public string order_id { get; set; }
        public string external_reference { get; set; }

        public OrderPaymentNotification(string order_id, string external_reference)
        {
            this.order_id = order_id;
            this.external_reference = external_reference;
        }
    }
}
