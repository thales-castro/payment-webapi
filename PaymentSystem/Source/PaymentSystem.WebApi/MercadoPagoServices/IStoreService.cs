namespace PaymentSystem.WebApi.MercadoPagoServices;

public interface IStoreService
{
    Task<bool> SetExternalIdAsync(string mpInternalUserId, int mpInternalStoreId, string token, string storeExternalIdToSet);
}
