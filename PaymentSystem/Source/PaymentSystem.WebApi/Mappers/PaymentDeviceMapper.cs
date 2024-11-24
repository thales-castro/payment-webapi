using PaymentSystem.WebApi.Dtos;
using PaymentSystem.WebApi.Dtos.Companies;
using PaymentSystem.WebApi.Entities;
using PaymentSystem.WebApi.ViewModels;
using System.Globalization;

namespace PaymentSystem.WebApi.Mappers;

public static class PaymentDeviceMapper
{
    public static double GetValueFromString(string value)
    {
        return value != null ? double.Parse(value, new CultureInfo("pt-BR")) : -1.0d;
    }

    public static string GetFromDouble(double value)
    {
        return value.ToString("N2", new CultureInfo("pt-BR"));
    }

    public static PaymentDevice GetEntityFromDto(PaymentDeviceDto dto)
    {
        return new()
        {
            CompanyId = dto.CompanyId,
            MacAddress = dto.MacAddress,
            CashierInternalMPId = dto.CashierInternalMPId,
            SellItemDescr = dto.SellItemDescr,
            SellItemValue = GetValueFromString(dto.SellItemValue)
        };
    }

    public static PaymentDeviceDto GetDtoFromEntity(PaymentDevice entity)
    {
        string value = "";
        if (entity != null &&
           entity.SellItemValue != -1)
        {
            value = GetFromDouble(entity.SellItemValue);
        }
        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            CompanyId = entity.CompanyId,
            MacAddress = entity.MacAddress ?? string.Empty,
            SellItemDescr = entity.SellItemDescr ?? string.Empty,
            SellItemValue = value
        };
    }

    public static void UpdateEntityFromDto(this PaymentDevice entity, PaymentDeviceDto dto)
    {
        entity.Name = dto.Name;
        entity.CompanyId = dto.CompanyId;
        entity.MacAddress = dto.MacAddress;
        entity.SellItemValue = GetValueFromString(dto.SellItemValue);
        entity.SellItemDescr = dto.SellItemDescr;
    }

    public static PaymentDeviceViewModel GetViewModelFromEntity(PaymentDevice entity, CompanyDto company)
    {
        string value = "";
        if (entity != null &&
           entity.SellItemValue != -1)
        {
            value = GetFromDouble(entity.SellItemValue);
        }

        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            CompanyId = company.Id ?? throw new Exception("Company Id não pode ser nulo ou vazio."), 
            CompanyName = company.Name ?? "Não cadastrado",
            MacAddress = entity.MacAddress ?? string.Empty,
            CashierInternalMPId = entity.CashierInternalMPId ?? string.Empty,
            SellItemDescr = entity?.SellItemDescr ?? string.Empty,
            SellItemValue = value
        };
    }
}
