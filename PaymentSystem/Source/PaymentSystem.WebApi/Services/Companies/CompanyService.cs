using MongoDB.Bson;
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
        CheckNullOrEmptyDataInCompany(dto, false);
        await CheckDuplicateDataInCompany(dto.Cnpj!, (int)dto.MpStoreInternalId!);

        var entity = CompanyMapper.GetEntityFromDto(dto);
        entity.Cnpj = entity.Cnpj.Replace(".", "");
        entity.Cnpj = entity.Cnpj.Replace("/", "");
        entity.Cnpj = entity.Cnpj.Replace("-", "");
        entity.Id = ObjectId.GenerateNewId().ToString();

        await _storeService.SetExternalIdAsync(entity.MpUserId, entity.MpStoreInternalId!, entity.Token, entity.Id);

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
        CheckNullOrEmptyDataInCompany(dto, true);

        var entity = await _repository.GetByIdAsync(dto.Id!.ToString()) ??
            throw new Exception($"Empresa com Id {dto.Id} não encontrada.");

        entity.UpdateEntityFromDto(dto);

        _repository.Update(entity);

        return CompanyMapper.GetDtoFromEntity(entity);
    }

    public CompanyDto Delete(string id)
    {
        var entity = _repository.Delete(id);
        return CompanyMapper.GetDtoFromEntity(entity);
    }

    public async Task<string> GetNameByIdAsync(string id) =>
        await _repository.GetNameByIdAsync(id) ??
            throw new EntityNotFoundException($"Empresa com Id {id} não encontrada.");

    private static void CheckNullOrEmptyDataInCompany(CompanyDto dto, bool idCheck)
    {
        List<string>? nullOrEmptyDataNames = null;
        if (idCheck && string.IsNullOrEmpty(dto.Id))
        {
            nullOrEmptyDataNames ??= [];
            nullOrEmptyDataNames.Add(nameof(dto.Id));
        }

        if (string.IsNullOrEmpty(dto.Name))
        {
            nullOrEmptyDataNames ??= [];
            nullOrEmptyDataNames.Add("Nome");
        }
        if (string.IsNullOrEmpty(dto.Cnpj))
        {
            nullOrEmptyDataNames ??= [];
            nullOrEmptyDataNames.Add("CNPJ");
        }

        if (string.IsNullOrEmpty(dto.MpUserId))
        {
            nullOrEmptyDataNames ??= [];
            nullOrEmptyDataNames.Add("Usuário do Mercado Pago");
        }

        if (dto.MpStoreInternalId == null)
        {
            nullOrEmptyDataNames ??= [];
            nullOrEmptyDataNames.Add("Id Interno da Loja do Mercado Pago");
        }

        if (string.IsNullOrEmpty(dto.Token))
        {
            nullOrEmptyDataNames ??= [];
            nullOrEmptyDataNames.Add("Token do Mercado Pago");
        }

        if (nullOrEmptyDataNames != null && nullOrEmptyDataNames.Count > 0)
            throw new Exception($"Os seguintes dados não podem ser nulos/vazios: [{string.Join(',', nullOrEmptyDataNames)}]");
    }

    private async Task CheckDuplicateDataInCompany(string cnpj, int mpInternalStoreId)
    {
        List<string>? duplicateData = null;
        if (await _repository.ExistsWithSameCnpjAsync(cnpj))
        {
            duplicateData ??= [];
            duplicateData.Add("CNPJ");
        }

        if (await _repository.ExistsWithSameMpStoreInternalIdAsync(mpInternalStoreId))
        {
            duplicateData ??= [];
            duplicateData.Add("Id Interndo da Loja (Mercado Pago)");
        }

        if (duplicateData != null && duplicateData.Count > 0)
            throw new Exception($"Já existe uma empresa registrada com o(s) mesmo(s) dado(s): [{string.Join(',', duplicateData)}]");
    }
}
