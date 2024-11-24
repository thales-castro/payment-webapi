namespace PaymentSystem.WebApi.MercadoPagoServices;

public interface IStoreService
{
    Task SetExternalIdAsync(string mpInternalUserId, int mpInternalStoreId, string token, string storeExternalIdToSet);
}
