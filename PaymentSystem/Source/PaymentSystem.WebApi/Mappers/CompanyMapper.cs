using PaymentSystem.WebApi.Dtos.Companies;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Mappers;

public static class CompanyMapper
{
    public static Company GetEntityFromDto(CompanyDto dto) =>
        new()
        {
            Name = dto.Name,
            Cnpj = dto.Cnpj
        };


    public static CompanyDto GetDtoFromEntity(Company entity) =>
        new()
        {
            Name = entity.Name,
            Cnpj = entity.Cnpj
        };
}
