using PaymentSystem.WebApi.Dtos;
using PaymentSystem.WebApi.Dtos.Companies;
using PaymentSystem.WebApi.Entities;
using PaymentSystem.WebApi.ViewModels;

namespace PaymentSystem.WebApi.Mappers;

public static class PaymentDeviceMapper
{
    public static PaymentDevice GetEntityFromDto(PaymentDeviceDto dto) =>
        new()
        {
            CompanyId = dto.CompanyId,
            CashierExternalId = dto.CashierExternalId,
            MacAddress = dto.MacAddress
        };

    public static PaymentDeviceDto GetDtoFromEntity(PaymentDevice entity) =>
        new()
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            CashierExternalId = entity.CashierExternalId ?? string.Empty,
            MacAddress = entity.MacAddress ?? string.Empty
        };

    public static void UpdateEntityFromDto(this PaymentDevice entity, PaymentDeviceDto dto)
    {
        entity.CompanyId = dto.CompanyId;
        entity.CashierExternalId = dto.CashierExternalId;
        entity.MacAddress = dto.MacAddress;
    }

    public static PaymentDeviceViewModel GetViewModelFromEntity(PaymentDevice entity, CompanyDto company) =>
        new()
        {
            Id = entity.Id,
            CompanyId = company.Id ?? throw new Exception("CompanyId can't be empty."), 
            CompanyName = company.Name ?? "Não cadastrado",
            CashierExternalId = entity.CashierExternalId ?? string.Empty,
            MacAddress = entity.MacAddress ?? string.Empty
        };
}
