namespace PaymentWebApi.Dtos.MercadoPago;

public class MerchantOrderDto
{
    public long mp_id { get; set; }
    public string status { get; set; }
    public string external_reference { get; set; }
    public bool cancelled { get; set; }
    public string order_status { get; set; }

    public MerchantOrderDto(long mp_id, string status, string external_reference, bool cancelled, string order_status)
    {
        this.mp_id = mp_id;
        this.status = status;
        this.external_reference = external_reference;
        this.cancelled = cancelled;
        this.order_status = order_status;
    }
}