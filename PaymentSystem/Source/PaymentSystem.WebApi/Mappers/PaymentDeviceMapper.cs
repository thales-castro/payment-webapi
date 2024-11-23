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
            MacAddress = dto.MacAddress,
            CashierInternalMPId = dto.CashierInternalMPId
        };

    public static PaymentDeviceDto GetDtoFromEntity(PaymentDevice entity) =>
        new()
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            MacAddress = entity.MacAddress ?? string.Empty
        };

    public static void UpdateEntityFromDto(this PaymentDevice entity, PaymentDeviceDto dto)
    {
        entity.CompanyId = dto.CompanyId;
        entity.MacAddress = dto.MacAddress;
        entity.CashierInternalMPId = dto.CashierInternalMPId;
    }

    public static PaymentDeviceViewModel GetViewModelFromEntity(PaymentDevice entity, CompanyDto company) =>
        new()
        {
            Id = entity.Id,
            CompanyId = company.Id ?? throw new Exception("Company Id não pode ser nulo ou vazio."), 
            CompanyName = company.Name ?? "Não cadastrado",
            MacAddress = entity.MacAddress ?? string.Empty,
            CashierInternalMPId = entity.CashierInternalMPId ?? string.Empty
        };
}
