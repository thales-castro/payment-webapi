using PaymentSystem.WebApi.Dtos.MercadoPago;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Mappers;

public static class OrderMapper
{
    public static OrderDto GetDtoFromEntity(Order entity) =>
        new(entity.description,
            entity.external_reference,
            entity.notification_url,
            entity.title,
            entity.total_amount,
            entity.items);
}
