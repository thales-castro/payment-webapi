using MongoDB.Driver;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IMongoDatabase database, ILoggerFactory loggerFactory) : base(database, loggerFactory) 
        => CollectionName = "users";

    public async Task<User?> GetByIdAsync(string id) =>
        await GetCollection().Find(doc => doc.Id == id).FirstOrDefaultAsync();

    public async Task<User?> GetByUsernameAsync(string username) =>
        await GetCollection().Find(doc => doc.Username == username).FirstOrDefaultAsync();

    public async Task<List<User>?> GetByCompanyIdAsync(string companyId) =>
        await GetCollection().Find(doc => doc.CompanyId == companyId).ToListAsync();
}