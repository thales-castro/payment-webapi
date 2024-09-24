namespace PaymentWebApi.PaymentEntities
{
    public class Cashier
    {
        public string Id { get; set; }

        public Cashier(string id)
        {
            Id = id; 
        }
    }
}
