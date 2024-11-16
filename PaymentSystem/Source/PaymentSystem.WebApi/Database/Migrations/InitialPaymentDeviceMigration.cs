using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Extensions.Migration;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Migrations;

public class InitialPaymentDeviceMigration : IMigration
{
    public int Version => 2;

    private readonly IMongoCollection<Company> _companies;
    private readonly IMongoCollection<PaymentDevice> _paymentDevices;

    public InitialPaymentDeviceMigration(IMongoDatabase db)
    {
        _companies = db.GetCollection<Company>("companies");
        _paymentDevices = db.GetCollection<PaymentDevice>("payment_devices");
        Up();
    }

    public void Down(BsonDocument document)
    {
        throw new NotImplementedException();
    }

    public void Up()
    {
        /*
        var company = new Company
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = "Empresa de teste MP",
            Cnpj = "CNPJ de teste MP",
            MpUserId = "1994736709",
            MpStoreExternalReference = "AfeTestStore01",
            Token = "APP_USR-6099630597304134-092008-9b1e83a93622d940a234083eeeee63bd-1994736709",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "InitialDataCreation"
        };
        if (_companies.Find(c => c.Token == company.Token).FirstOrDefault() == null)
            _companies.InsertOne(company);

        var paymentDevice = new PaymentDevice
        {
            CompanyId = company.Id,
            MacAddress = "10:52:1c:5d:49:e8",
            CashierExternalId = "AfeTestCashier01",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "InitialDataCreation"
        };
        if (_paymentDevices.Find(pd => pd.MacAddress == paymentDevice.MacAddress).FirstOrDefault() == null)
            _paymentDevices.InsertOne(paymentDevice);
        */
    }

    public void Up(BsonDocument document)
    {
        throw new NotImplementedException();
    }
}
