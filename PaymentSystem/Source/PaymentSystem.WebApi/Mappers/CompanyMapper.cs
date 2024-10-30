using PaymentSystem.WebApi.Dtos.Companies;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Mappers;

public static class CompanyMapper
{
    public static Company GetEntityFromDto(CompanyDto dto) =>
        new()
        {
            Name = dto.Name,
            Cnpj = dto.Cnpj,
            MpUserId = dto.MpUserId,
            MpStoreExternalReference = dto.MpStoreExternalReference,
            Token = dto.Token
        };


    public static CompanyDto GetDtoFromEntity(Company entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name ?? string.Empty,
            Cnpj = entity.Cnpj ?? string.Empty,
            MpUserId = entity.MpUserId,
            MpStoreExternalReference = entity.MpStoreExternalReference,
            Token = entity.Token
        };
}
