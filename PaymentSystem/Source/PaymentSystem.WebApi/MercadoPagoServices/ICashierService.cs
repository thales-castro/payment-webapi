namespace PaymentSystem.WebApi.MercadoPagoServices;

public interface ICashierService
{
    Task SetExternalIdAsync(string storeExternalCashierIdToSet, string cashierName, string mpInternalCashierId, string token);
}
