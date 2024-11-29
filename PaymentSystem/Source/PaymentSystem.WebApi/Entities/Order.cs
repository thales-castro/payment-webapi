using MongoDB.Bson;
using System.Text.Json;

namespace PaymentSystem.WebApi.Entities;

public enum OrderStatus
{
    OPEN,
    PAID,
    EXPIRED,
    RETURNED,
    WAITING_PAID
}

public class Order : BaseEntity
{
    public Order(string mac_address, string description, string notification_url, string title, double total_amount, Item[] items)
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

    public static string GetDescription(OrderStatus status)
    {
        switch(status)
        {
            case OrderStatus.OPEN:
                return "Aberta";
            case OrderStatus.PAID:
                return "Pago";
            case OrderStatus.RETURNED:
                return "Retornou Pago Placa";
            case OrderStatus.EXPIRED:
                return "Expirada";
            case OrderStatus.WAITING_PAID:
                return "Aguardando Pagamento";
        }
        return string.Empty;
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
            "http://173.249.14.50:5228/AfePayment", // TODO: Vai ser o IP do server e a porta (HARDCODED, no futuro envvar)
            // "http://afepay.ddns.net:8089/AfePayment", // TODO: Vai ser o IP do server e a porta (HARDCODED, no futuro envvar)
            "AfePayment Order",
            1,
            default_items
            );
        return default_order;
    }

    public static Order CreateNewOrder(string mac_address, string itemDescription, double value)
    {
        Item item = new Item(itemDescription, value, 1, "Real", value);
        Item[] items = [item];
        Order newOrder = new Order(
            mac_address,
            "AfePayment Order",
            "http://173.249.14.50:5228/AfePayment", // TODO: Vai ser o IP do server e a porta (HARDCODED, no futuro envvar)
            //"http://afepay.ddns.net:8089/AfePayment", // TODO: Vai ser o IP do server e a porta (HARDCODED, no futuro envvar)
            "AfePayment Order",
            value,
            items
            );
        return newOrder;
    }

    public string description { get; set; }
    public string external_reference { get; set; }
    public string notification_url { get; set; }
    public string title { get; set; }
    public double total_amount { get; set; }
    public Item[] items { get; set; }
    public string mac_address { get; set; }
    public OrderStatus status { get; set; }
    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}
