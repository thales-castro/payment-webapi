namespace PaymentWebApi.PaymentEntities
{
    public class Store
    {
        public string Id { get; set; }
        public List<Cashier> Cashier { get; set; }

        public Store(string id, List<Cashier> cashier)
        {
            Id = id;
            Cashier = cashier;
        }
    }
}
