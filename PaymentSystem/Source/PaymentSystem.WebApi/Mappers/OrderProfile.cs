using AutoMapper;
using PaymentSystem.WebApi.Dtos.MercadoPago;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Mappers;

public class OrderProfile : Profile
{
    public OrderProfile() 
    {
        CreateMap<Order, OrderDto>();
    }
}
