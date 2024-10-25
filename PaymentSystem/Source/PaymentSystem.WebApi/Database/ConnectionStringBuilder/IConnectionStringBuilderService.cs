namespace PaymentSystem.WebApi.Database.ConnectionStringBuilder;

public interface IConnectionStringBuilderService
{
    string GenerateMongConnection(out string databaseName);
}
