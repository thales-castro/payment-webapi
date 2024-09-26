using MongoDB.Bson;
using System.Text.Json;

namespace PaymentWebApi.Entities;

public enum OrderStatus
{
    OPEN,
    PAID,
    EXPIRED,
    RETURNED
}

public class Order : BaseEntity
{
    public Order(string mac_address, string description, string notification_url, string title, int total_amount, Item[] items)
    {
        this.Id = ObjectId.GenerateNewId().ToString();
        this.external_reference = this.Id;
        this.description = description;        
        this.notification_url = notification_url;
        this.title = title;
        this.total_amount = total_amount;
        this.items = items;
        this.mac_address = mac_address;
        this.status = OrderStatus.OPEN;
    }

    public static Order LoadDefault(string mac_address)
    {
        //TODO: Com a interface administrativa controlar melhor o produto de acordo
        // com o cliente.
        Item default_item = new Item("Default Item", 1, 1, "unit", 1);
        Item[] default_items = [default_item];
        Order default_order = new Order(
            mac_address,
            "AfePayment Default Order",
            "http://afepay.ddns.net:8089/AfePayment",
            "AfePayment Order",
            1,
            default_items
            );
        return default_order;
    }

    public string description { get; set; }
    public string external_reference { get; set; }
    public string notification_url { get; set; }
    public string title { get; set; }
    public int total_amount { get; set; }
    public Item[] items { get; set; }
    public string mac_address { get; set; }
    public OrderStatus status { get; set; }
    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}
