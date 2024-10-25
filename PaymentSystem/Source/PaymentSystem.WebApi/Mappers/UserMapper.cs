using PaymentSystem.WebApi.Dtos.Users;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Mappers;

public static class UserMapper
{
    public static UserOutDto GetDtoFromEntity(User entity) => 
        new() 
        {
            Name = entity.Name,
            Username = entity.Username,
            CompanyId = entity.CompanyId,
            Email = entity.Email,
            Id = entity.Id,
            IsRemoved = entity.IsRemoved,
            UserType = entity.UserType
        };
}
