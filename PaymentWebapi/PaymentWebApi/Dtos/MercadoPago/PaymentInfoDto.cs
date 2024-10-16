namespace PaymentWebApi.Dtos.MercadoPago;

public class PaymentOrderDto
{
    public PaymentOrderDto(string id, string type)
    {
        this.id = id;
        this.type = type;
    }

    public string id { get; set; }
    public string type { get; set; }
}

public class PaymentInfoDto
{
    public PaymentInfoDto(string date_approved, string date_created, string date_last_updated, 
        string description, string external_reference, string money_release_date, 
        string notification_url, PaymentOrderDto order, string status, string store_id)
    {
        this.date_approved = date_approved;
        this.date_created = date_created;
        this.date_last_updated = date_last_updated;
        this.description = description;
        this.external_reference = external_reference;
        this.money_release_date = money_release_date;
        this.notification_url = notification_url;
        this.order = order;
        this.status = status;
        this.store_id = store_id;
    }

    public string date_approved { get; set; }
    public string date_created { get; set; }
    public string date_last_updated { get; set; }
    public string description { get; set; }
    public string external_reference { get; set; }
    public string money_release_date { get; set; }
    public string notification_url { get; set; }
    public PaymentOrderDto order { get; set; }
    public string status { get; set; }
    public string store_id { get; set; }
}
