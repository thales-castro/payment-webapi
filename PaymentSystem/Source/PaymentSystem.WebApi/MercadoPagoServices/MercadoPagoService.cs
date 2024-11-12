namespace PaymentSystem.WebApi.MercadoPagoServices;

public class MercadoPagoService
{
    protected readonly HttpClient _httpClient;
    protected readonly IConfiguration _configuration;

    public MercadoPagoService(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    protected void SetBearerToken(string token) =>
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
}
