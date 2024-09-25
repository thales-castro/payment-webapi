using MongoDB.Driver;
using PaymentWebApi.Database.ConnectionStringBuilder;

namespace PaymentWebApi.Database;

public static class MongoInjection
{
    public static void AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionStringBuilderService, ConnectionStringBuilderService>();
        var connectionString = services.BuildServiceProvider().
            GetRequiredService<IConnectionStringBuilderService>().GenerateMongConnection(out string databaseName);
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase(databaseName);
        services.AddSingleton(database);
    }
}
