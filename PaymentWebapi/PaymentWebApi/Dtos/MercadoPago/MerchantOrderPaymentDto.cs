namespace PaymentWebApi.Dtos.MercadoPago;

public class MerchantOrderPaymentDto
{
    public MerchantOrderPaymentDto(long id, string order_id, string external_reference, string merchant_order_id, string status)
    {
        this.id = id;
        this.order_id = order_id;
        this.external_reference = external_reference;
        this.merchant_order_id = merchant_order_id;
        this.status = status;
    }

    public long id { get; set; }
    public string order_id { get; set; }
    public string external_reference { get; set; }
    public string merchant_order_id { get; set; }
    public string status { get; set; }
}


