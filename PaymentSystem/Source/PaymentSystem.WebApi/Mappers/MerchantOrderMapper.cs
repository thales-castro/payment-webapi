
using PaymentSystem.WebApi.Dtos.MercadoPago;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Mappers;

public static class MerchantOrderMapper
{
    public static MerchantOrder GetEntityFromDto(MerchantOrderDto dto) =>
        new(dto.mp_id, dto.status, dto.external_reference, dto.cancelled, dto.order_status);
}
