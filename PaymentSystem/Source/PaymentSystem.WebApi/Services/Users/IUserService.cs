using PaymentSystem.WebApi.Dtos.Users;
using PaymentSystem.WebApi.Entities;

namespace PaymentSystem.WebApi.Services.Users;

public interface IUserService
{
    UserOutDto Register(UserInDto dto);
    List<UserOutDto> GetAll();
    Task<User?> GetByUsernameAsync(string username);
    Task<UserOutDto> GetByIdAsync(Guid id);
    Task<UserOutDto> UpdateAsync(UserInDto dto);
    UserOutDto Delete(Guid id);
    User Update(User user);
    Task<List<User>?> GetUserByCompanyIdAsync(Guid companyId);
}
