using PaymentWebApi.Entities;
using System.Text.Json;

namespace PaymentWebApi.Dtos.MercadoPago
{
    public class OrderDto
    {
        public OrderDto(string description, string external_reference, string notification_url, string title, int total_amount, Item[] items)
        {
            this.description = description;
            this.external_reference = external_reference;
            this.notification_url = notification_url;
            this.title = title;
            this.total_amount = total_amount;
            this.items = items;
        }

        public string description { get; set; }
        public string external_reference { get; set; }
        public string notification_url { get; set; }
        public string title { get; set; }
        public int total_amount { get; set; }
        public Item[] items { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
