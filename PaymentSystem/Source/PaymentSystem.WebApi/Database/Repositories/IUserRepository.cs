using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByUsernameAsync(string username);
    Task<List<User>?> GetByCompanyIdAsync(string companyId);
}
