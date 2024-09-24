namespace PaymentWebApi.Database.ConnectionStringBuilder;

public interface IConnectionStringBuilderService
{
    string GenerateMongConnection(out string databaseName);
}
