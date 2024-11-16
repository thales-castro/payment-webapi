using MongoDB.Driver;
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
            MpStoreInternalId = dto.MpStoreInternalId,
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
            MpStoreInternalId = entity.MpStoreInternalId,
            MpStoreExternalReference = entity.MpStoreExternalReference,
            Token = entity.Token
        };

    public static void UpdateEntityFromDto(this Company entity, CompanyDto dto)
    {
        entity.Cnpj = dto.Cnpj;
        entity.Name = dto.Name;
        entity.MpUserId = dto.MpUserId;
        entity.Token = dto.Token;
    }
}
