namespace PaymentSystem.WebApi.MercadoPagoServices;

public interface ICashierService
{
    Task<bool> SetExternalIdAsync(string mpInternalCashierId, string storeExternalCashierIdToSet, string token);
}
