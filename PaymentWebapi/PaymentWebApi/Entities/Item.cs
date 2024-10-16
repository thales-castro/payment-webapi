using System.Text.Json;

namespace PaymentWebApi.Entities
{
    public class Item
    {
        public string title { get; set; }
        public int unit_price { get; set; }
        public int quantity { get; set; }
        public string unit_measure { get; set; }
        public int total_amount { get; set; }

        public Item(string title, int unit_price, int quantity, string unit_measure, int total_amount)
        {
            this.title = title;
            this.unit_price = unit_price;
            this.quantity = quantity;
            this.unit_measure = unit_measure;
            this.total_amount = total_amount;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
