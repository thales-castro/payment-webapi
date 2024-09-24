using PaymentWebApi.Database;
using PaymentWebApi.PaymentEntities;

namespace PaymentWebApi.Dao
{
    public class OrderDao
    {
        private FakeDatabase _database;

        public OrderDao()
        {
            _database = FakeDatabase.Get();
        }

        public Order LoadDefault()
        {
            Item default_item = new Item("Ursinho", 1, 1, "unit", 1);
            Item[] default_ints = [default_item];

            Order default_order = new Order(
                "AfePayment Default Order",
                "12345",
                "http://afepay.ddns.net:8089/AfePayment",
                "AfePayment Order",
                1,
                default_ints
                );
            return default_order;
        }

        public void InsertOrder(Order order )
        {
            if (_database.OrderExists(order.external_reference))
            {
                _database.DeleteOrder(order.external_reference);
                if(_database.MerchantOrderExists(order.external_reference))
                    _database.DeleteMerchantOrder(order.external_reference);    
            }
            _database.InsertOrder(order);
        }

        public bool OrderIsPaid(string external_reference)
        {
            if(_database.MerchantOrderExists(external_reference)) 
                return _database.GetMerchantOrder(external_reference)?.status == "paid";
            return false;
        }

        public void InsertMerchantOrder(MerchantOrder merchantOrder)
        {
            _database.InserMerchantOrder(merchantOrder);
        }

        public void SetMerchantPaid(string external_reference)
        {
            _database.SetMerchantPaid(external_reference);
        }
    }
}
