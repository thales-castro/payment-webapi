using PaymentSystem.WebApi.Database.Repositories;
using PaymentSystem.WebApi.Dtos.Companies;
using PaymentSystem.WebApi.Exceptions;
using PaymentSystem.WebApi.Mappers;
using PaymentSystem.WebApi.MercadoPagoServices;

namespace PaymentSystem.WebApi.Services.Companies;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repository;
    private readonly IStoreService _storeService;

    public CompanyService(ICompanyRepository repository, IStoreService storeService)
    {
        _repository = repository;
        _storeService = storeService;
    }

    public async Task<CompanyDto> Register(CompanyDto dto)
    {
        var entity = CompanyMapper.GetEntityFromDto(dto);
        entity.Cnpj = entity.Cnpj?.Replace(".", "");
        entity.Cnpj = entity.Cnpj?.Replace("/", "");
        entity.Cnpj = entity.Cnpj?.Replace("-", "");

        if (!string.IsNullOrEmpty(dto.MpUserId) && 
            dto.MpStoreInternalId != null && 
            !string.IsNullOrEmpty(dto.MpStoreExternalReference) &&
            !string.IsNullOrEmpty(dto.Token))
        {
            if (await _storeService.SetExternalIdAsync(dto.MpUserId, (int)dto.MpStoreInternalId, dto.Token, dto.MpStoreExternalReference))
                entity.MpStoreExternalReference = dto.MpStoreExternalReference;
        }

        _repository.Create(entity);
        return CompanyMapper.GetDtoFromEntity(entity);
    }

    public async Task<CompanyDto> GetByIdAsync(string id)
    {
        var entity = await _repository.GetByIdAsync(id) ??
            throw new EntityNotFoundException($"Company with id [{id}] not found.");
        return CompanyMapper.GetDtoFromEntity(entity); ;
    }

    public List<CompanyDto> GetAll()
    {
        var entities = _repository.ReadAll();

        var dtos = new List<CompanyDto>();
        foreach (var entity in entities)
            dtos.Add(CompanyMapper.GetDtoFromEntity(entity));

        return dtos;
    }

    public async Task<List<CompanyDto>> GetNotDeleted()
    {
        var entities = await _repository.GetNotDeletedAsync();

        var dtos = new List<CompanyDto>();
        foreach (var entity in entities)
            dtos.Add(CompanyMapper.GetDtoFromEntity(entity));

        return dtos;
    }

    public async Task<CompanyDto> UpdateAsync(CompanyDto dto)
    {
        if (dto.Id == null)
            throw new Exception("Id can't be null.");

        var entity = await _repository.GetByIdAsync(dto.Id.ToString()) ??
            throw new Exception($"Could not find Company with id {dto.Id}");
        entity.UpdateEntityFromDto(dto);

        if (!string.IsNullOrEmpty(entity.MpUserId) &&
            entity.MpStoreInternalId != null &&
            !string.IsNullOrEmpty(dto.MpStoreExternalReference) &&
            dto.MpStoreExternalReference != entity.MpStoreExternalReference &&
            !string.IsNullOrEmpty(dto.Token))
        {
            if (await _storeService.SetExternalIdAsync(dto.MpUserId!, (int)dto.MpStoreInternalId!, dto.Token, dto.MpStoreExternalReference))
                entity.MpStoreExternalReference = dto.MpStoreExternalReference;
        }

        _repository.Update(entity);
        return CompanyMapper.GetDtoFromEntity(entity);
    }

    public CompanyDto Delete(string id)
    {
        var entity = _repository.Delete(id);
        var dto = CompanyMapper.GetDtoFromEntity(entity) ??
            throw new EntityNotFoundException($"Company with id [{id}] not found.");
        return dto;
    }

    public async Task<string> GetNameByIdAsync(string id) =>
        await _repository.GetNameByIdAsync(id) ??
            throw new EntityNotFoundException($"Company with id [{id}] not found.");
}
