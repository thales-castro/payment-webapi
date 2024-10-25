using PaymentSystem.WebApi.Dtos.Companies;

namespace PaymentSystem.WebApi.Services.Companies;

public interface ICompanyService
{
    CompanyDto Register(CompanyDto dto);
    List<CompanyDto> GetAll();
    Task<CompanyDto> GetByIdAsync(Guid id);
    Task<CompanyDto> UpdateAsync(CompanyDto dto);
    CompanyDto Delete(Guid id);
    Task<List<CompanyDto>> GetNotDeleted();
    Task<string> GetNameByIdAsync(string id);
}
