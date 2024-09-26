using MongoDB.Driver;
using MongoDB.Extensions.Migration;
using PaymentWebApi.Database.ConnectionStringBuilder;
using PaymentWebApi.Database.Migrations;
using PaymentWebApi.Entities;

namespace PaymentWebApi.Database;

public static class MongoInjection
{
    public static void AddDatabase(this IServiceCollection services)
    {
        var connectionString = services.BuildServiceProvider().
            GetRequiredService<IConnectionStringBuilderService>().GenerateMongConnection(out string databaseName);
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase(databaseName);
        services.AddSingleton(database);
    }

    public static void AddDefaultMongoData(this IApplicationBuilder app, IServiceCollection services)
    {
        var database = services.BuildServiceProvider().
            GetRequiredService<IMongoDatabase>();

        app.UseMongoMigration(m =>
            m.ForEntity<PaymentDevice>(e => e.AtVersion(1).WithMigration(new InitialPaymentDeviceMigration(database))));
    }
}
