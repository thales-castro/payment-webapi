using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Extensions.Migration;
using PaymentWebApi.Entities;

namespace PaymentWebApi.Database.Migrations;

public class InitialPaymentDeviceMigration : IMigration
{
    public int Version => 1;

    private readonly IMongoCollection<PaymentDevice> _paymentDevices;

    public InitialPaymentDeviceMigration(IMongoDatabase db)
    {
        _paymentDevices = db.GetCollection<PaymentDevice>("payment_devices");
        Up();
    }

    public void Down(BsonDocument document)
    {
        throw new NotImplementedException();
    }

    public void Up()
    {
        var entity = new PaymentDevice
        {
            MacAddress = "10:52:1c:5d:49:e8",
            UserId = 1994736709,
            StoreExternalId = "AfeTestStore01",
            CashierExternalId = "AfeTestCashier01"
        };
        if(_paymentDevices.Find(pd => pd.MacAddress == entity.MacAddress).FirstOrDefault() == null)
            _paymentDevices.InsertOne(entity);
    }

    public void Up(BsonDocument document)
    {
        throw new NotImplementedException();
    }
}
