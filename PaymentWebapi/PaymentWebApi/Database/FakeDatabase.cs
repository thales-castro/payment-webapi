using PaymentWebApi.PaymentEntities;

namespace PaymentWebApi.Database
{
    public class FakeDatabase
    {
        private static FakeDatabase? _instance;
        private Dictionary<string, Order> _orders;
        private Dictionary<string, MerchantOrder> _merchantOrders;

        private FakeDatabase() 
        {
            _orders = new Dictionary<string, Order>();
            _merchantOrders = new Dictionary<string, MerchantOrder>();
        }

        public static FakeDatabase Get()
        {
            if (_instance == null)
                _instance = new FakeDatabase();
            return _instance;
        }

        public void InsertOrder(Order order)
        {
            _orders.Add(order.external_reference, order);
        }

        public bool OrderExists(string external_reference)
        {
            return _orders.ContainsKey(external_reference);
        }

        public void DeleteOrder(string external_reference)
        {
            _orders.Remove(external_reference);
        }

        public void InserMerchantOrder(MerchantOrder merchantOrder)
        {
            if(!_merchantOrders.ContainsKey(merchantOrder.external_reference))
                _merchantOrders.Add(merchantOrder.external_reference, merchantOrder);
        }

        public bool MerchantOrderExists(string external_reference)
        {
            return _merchantOrders.ContainsKey(external_reference);
        }

        public void DeleteMerchantOrder(string external_reference)
        {
            _merchantOrders.Remove(external_reference);
        }

        public MerchantOrder? GetMerchantOrder(string external_reference)
        {
            if(_merchantOrders.ContainsKey(external_reference))
                return _merchantOrders[external_reference];
            return null;
        }

        public void SetMerchantPaid(string external_reference)
        {
            if (_merchantOrders.ContainsKey(external_reference))
                _merchantOrders[external_reference].status = "paid";
        }

    }
}
