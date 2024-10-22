using PaymentSystem.WebApi.Dtos.MercadoPago;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Mappers;

public static class PaymentInfoMapper
{
    public static PaymentInfo GetEntityFromDto(PaymentInfoDto dto) =>
        new()
        {
            date_approved = dto.date_approved,
            date_created = dto.date_created,
            date_last_updated = dto.date_last_updated,
            description = dto.description,
            external_reference = dto.external_reference,
            money_release_date = dto.money_release_date,
            notification_url = dto.notification_url,
            status = dto.status,
            store_id = dto.store_id,
            payment_order_id = dto.order.id,
            payment_order_type = dto.order.type
        };
}
