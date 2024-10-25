using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Database.Repositories;

public interface ICompanyRepository : IGenericRepository<Company>
{
    Task<string> GetNameByIdAsync(string id);
    Task<Company> GetByIdAsync(string id);
    Task<List<Company>> GetNotDeletedAsync();
}
