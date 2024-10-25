namespace PaymentSystem.WebApi.MercadoPagoServices;

public class MercadoPagoService
{
    //TODO: Colocar no appsettings ou esse cara é específico para cada usuário?
    //private readonly string _token = "APP_USR-6099630597304134-092008-9b1e83a93622d940a234083eeeee63bd-1994736709";
    protected HttpClient _httpClient;

    public MercadoPagoService()
    {
        _httpClient = new HttpClient();
        //_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
    }
}
