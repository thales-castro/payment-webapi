namespace PaymentWebApi.Database.ConnectionStringBuilder;

public class ConnectionStringBuilderService : IConnectionStringBuilderService
{
    private readonly IConfiguration _configuration;

    public ConnectionStringBuilderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateMongConnection(out string databaseName)
    {
        databaseName = _configuration["DatabaseSettings:MongoDb:DatabaseName"] ?? "payment_webapi";
        //mongodb://admin:payment@localhost:27017/tracker_system?authSource=admin - LOCAL DOCKER CONTAINER
        var serverPrefix = _configuration["DatabaseSettings:MongoDb:ServerPrefix"];
        string connStr = string.Format(
            "{0}://{1}:{2}@{3}/",
            _configuration["DatabaseSettings:MongoDb:ServerPrefix"], // 0
            _configuration["DatabaseSettings:MongoDb:Username"], // 1
            _configuration["DatabaseSettings:MongoDb:Password"], // 2
            _configuration["DatabaseSettings:MongoDb:Host"]); // 3

        if (serverPrefix == "mongodb+srv")
            connStr += "?retryWrites=true&w=majority";
        else if (serverPrefix == "mongodb")
            connStr += $"{databaseName}?authSource={_configuration["DatabaseSettings:MongoDb:Username"]}";

        return connStr;
    }
}
