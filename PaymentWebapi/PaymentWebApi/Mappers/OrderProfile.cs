using AutoMapper;
using PaymentWebApi.Dtos.MercadoPago;
using PaymentWebApi.Entities;

namespace PaymentWebApi.Mappers;

public class OrderProfile : Profile
{
    public OrderProfile() 
    {
        CreateMap<Order, OrderDto>();
    }
}
