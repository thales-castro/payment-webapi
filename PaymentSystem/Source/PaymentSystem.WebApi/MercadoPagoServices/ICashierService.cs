namespace PaymentSystem.WebApi.MercadoPagoServices;

public interface ICashierService
{
    Task SetExternalIdAsync(string mpInternalCashierId, string storeExternalCashierIdToSet, string token);
}
