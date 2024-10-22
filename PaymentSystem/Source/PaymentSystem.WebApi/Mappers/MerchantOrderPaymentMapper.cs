using PaymentSystem.WebApi.Dtos.MercadoPago;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Mappers;

public static class MerchantOrderPaymentMapper
{
    public static MerchantOrderPayment GetEntityFromDto(MerchantOrderPaymentDto dto) =>
        new(dto.id, dto.order_id, dto.external_reference, dto.merchant_order_id, dto.status);
}
