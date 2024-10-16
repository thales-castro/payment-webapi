
using PaymentWebApi.Dtos.MercadoPago;
using PaymentWebApi.Entities;

namespace PaymentWebApi.Mappers;

public static class MerchantOrderMapper
{
    public static MerchantOrder GetEntityFromDto(MerchantOrderDto dto) =>
        new(dto.mp_id, dto.status, dto.external_reference, dto.cancelled, dto.order_status);
}
