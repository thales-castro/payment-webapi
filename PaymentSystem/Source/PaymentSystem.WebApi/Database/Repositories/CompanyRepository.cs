using MongoDB.Driver;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
{
    public CompanyRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory)
        => CollectionName = "companies";

    public async Task<string?> GetNameByIdAsync(string id) =>
        (await GetCollection().Find(doc => doc.Id == id).FirstOrDefaultAsync()).Name;

    public async Task<Company> GetByIdAsync(string id) =>
        await GetCollection().Find(doc => doc.Id == id).FirstOrDefaultAsync();

    public async Task<List<Company>> GetNotDeletedAsync() =>
        await GetCollection().Find(doc => !doc.IsRemoved).ToListAsync();

    public async Task<Company> GetByMpUserIdAsync(string mpUserId) =>
        await GetCollection().Find(doc => doc.MpUserId == mpUserId).FirstOrDefaultAsync();

    public async Task<bool> ExistsWithSameCnpjAsync(string cnpj) =>
        await GetCollection().CountDocumentsAsync(doc => doc.Cnpj == cnpj) > 0;

    public async Task<bool> ExistsWithSameMpStoreInternalIdAsync(int pStoreInternalId) =>
        await GetCollection().CountDocumentsAsync(doc => doc.MpStoreInternalId == pStoreInternalId) > 0;
}

