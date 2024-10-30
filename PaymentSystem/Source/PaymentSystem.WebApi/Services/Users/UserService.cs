using PaymentSystem.WebApi.Database.Repositories;
using PaymentSystem.WebApi.Dtos.Users;
using PaymentSystem.WebApi.Entities;
using PaymentSystem.WebApi.Enums;
using PaymentSystem.WebApi.Exceptions;
using PaymentSystem.WebApi.Extensions;
using PaymentSystem.WebApi.Mappers;

namespace PaymentSystem.WebApi.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository) =>
        _repository = repository;

    public UserOutDto Register(UserInDto dto)
    {
        CriptographyExtension.GetPasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var entity = new User
        {
            Name = dto.Name,
            Username = dto.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Email = dto.Email,
            UserType = Enum.GetName(typeof(UserType), dto.UserType) ?? string.Empty,
            CompanyId = dto.CompanyId.ToString()
        };
        _repository.Create(entity);
        return UserMapper.GetDtoFromEntity(entity);
    }

    public List<UserOutDto> GetAll()
    {
        var entitiesList = _repository.ReadAll();
        var dtoList = new List<UserOutDto>();
        foreach (var entity in entitiesList)
            dtoList.Add(UserMapper.GetDtoFromEntity(entity));
        return dtoList;
    }

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _repository.GetByUsernameAsync(username);

    public async Task<UserOutDto> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id.ToString()) ??
            throw new EntityNotFoundException($"User with id [{id}] not found.");
        return UserMapper.GetDtoFromEntity(entity);
    }

    public async Task<UserOutDto> UpdateAsync(UserInDto dto)
    {
        if (dto.Id == null)
            throw new Exception("Id can't be null.");

        var user = await _repository.GetByIdAsync(dto.Id.ToString()) ??
            throw new Exception($"User with Id: {dto.Id} not found");

        //TODO: Check the business rules for update user
        user.Username = dto.Username;
        user.Name = dto.Name;

        _repository.Update(user);
        return UserMapper.GetDtoFromEntity(user);
    }

    public UserOutDto Delete(Guid id)
    {
        var removedEntity = _repository.Delete(id.ToString()) ?? 
            throw new EntityNotFoundException($"User with id [{id}] not found.");

        return UserMapper.GetDtoFromEntity(removedEntity);
    }

    public User Update(User user) =>
        _repository.Update(user);

    public async Task<List<User>?> GetUserByCompanyIdAsync(Guid companyId) =>
        await _repository.GetByCompanyIdAsync(companyId.ToString()) ??
            throw new EntityNotFoundException($"User with company id [{companyId}] not found.");
}
