using MongoDB.Bson;
using PaymentSystem.WebApi.Database.Repositories;
using PaymentSystem.WebApi.Dtos;
using PaymentSystem.WebApi.Exceptions;
using PaymentSystem.WebApi.Mappers;
using PaymentSystem.WebApi.MercadoPagoServices;
using PaymentSystem.WebApi.Services.Companies;
using PaymentSystem.WebApi.ViewModels;

namespace PaymentSystem.WebApi.Services.PaymentDevices;

public class PaymentDeviceService : IPaymentDeviceService
{
    private readonly IPaymentDeviceRepository _repository;
    private readonly ICompanyService _companyService;
    private readonly ICashierService _cashierService;

    public PaymentDeviceService(IPaymentDeviceRepository repository, ICompanyService companyService, ICashierService cashierService)
    {
        _repository = repository;
        _companyService = companyService;
        _cashierService = cashierService;
    }

    public async Task<PaymentDeviceDto> RegisterAsync(PaymentDeviceDto dto)
    {
        CheckNullOrEmptyDataInPaymentDevice(dto, false);
        await CheckDuplicateDataInPaymentDevice(dto.MacAddress, dto.CashierInternalMPId);

        var entity = PaymentDeviceMapper.GetEntityFromDto(dto);
        entity.Id = ObjectId.GenerateNewId().ToString();

        // TODO: No futuro, retornar o Token da empresa junto ao Id e Nome no Front, pois evita uma ida
        // ao banco para buscá-lo.
        var company = await _companyService.GetByIdAsync(entity.CompanyId);
        if (company.Token == null)
            throw new Exception("O Token da Empresa em que o dispositivo será vinculado é nulo.");

        await _cashierService.SetExternalIdAsync(entity.CashierInternalMPId!, entity.Id, company.Token);

        _repository.Create(entity);
        return PaymentDeviceMapper.GetDtoFromEntity(entity);
    }

    public async Task<PaymentDeviceDto> GetByIdAsync(string id)
    {
        var entity = await _repository.GetByIdAsync(id) ??
            throw new EntityNotFoundException($"Company with id [{id}] not found.");
        return PaymentDeviceMapper.GetDtoFromEntity(entity); ;
    }

    public List<PaymentDeviceDto> GetAll()
    {
        var entities = _repository.ReadAll();
        var dtos = new List<PaymentDeviceDto>();
        foreach (var entity in entities)
            dtos.Add(PaymentDeviceMapper.GetDtoFromEntity(entity));
        return dtos;
    }

    public async Task<PaymentDeviceDto> UpdateAsync(PaymentDeviceDto dto)
    {
        CheckNullOrEmptyDataInPaymentDevice(dto, true);

        var entity = await _repository.GetByIdAsync(dto.Id.ToString()) ??
            throw new Exception($"Empresa com Id {dto.Id} não encontrada.");

        entity.UpdateEntityFromDto(dto);

        _repository.Update(entity);

        return PaymentDeviceMapper.GetDtoFromEntity(entity);
    }

    public PaymentDeviceDto Delete(string id)
    {
        var entity = _repository.Delete(id);
        return PaymentDeviceMapper.GetDtoFromEntity(entity);
    }

    public async Task<List<PaymentDeviceViewModel>> GetNotDeletedAsync()
    {
        var entities = await _repository.GetNotDeletedAsync();

        var dtos = new List<PaymentDeviceViewModel>();
        foreach (var entity in entities)
        {
            var company = await _companyService.GetByIdAsync(entity.CompanyId) ??
                throw new EntityNotFoundException($"Device Company with Id {entity.CompanyId} not found.");
            dtos.Add(PaymentDeviceMapper.GetViewModelFromEntity(entity, company));
        }

        return dtos;
    }

    private async Task CheckDuplicateDataInPaymentDevice(string macAddress, string cashierInternalMPId)
    {
        List<string>? duplicateData = null;
        if (await _repository.CheckIfMacExistsAsync(macAddress))
        {
            duplicateData ??= [];
            duplicateData.Add("Endereço MAC");
        }

        if (await _repository.CheckIfCashierInternalMPIdExistsAsync(cashierInternalMPId))
        {
            duplicateData ??= [];
            duplicateData.Add("Id Interno do Caixa do Mercado Pago");
        }

        if (duplicateData != null && duplicateData.Count > 0)
            throw new Exception($"Já existe uma empresa registrada com o(s) mesmo(s) dado(s): [{string.Join(',', duplicateData)}]");
    }

    private static void CheckNullOrEmptyDataInPaymentDevice(PaymentDeviceDto dto, bool idCheck)
    {
        List<string>? nullOrEmptyDataNames = null;
        if (idCheck && string.IsNullOrEmpty(dto.Id))
        {
            nullOrEmptyDataNames ??= [];
            nullOrEmptyDataNames.Add(nameof(dto.Id));
        }

        if (string.IsNullOrEmpty(dto.CompanyId))
        {
            nullOrEmptyDataNames ??= [];
            nullOrEmptyDataNames.Add("Vínculo com Empresa");
        }

        if (string.IsNullOrEmpty(dto.MacAddress))
        {
            nullOrEmptyDataNames ??= [];
            nullOrEmptyDataNames.Add("Endereço MAC");
        }

        if (string.IsNullOrEmpty(dto.CashierInternalMPId))
        {
            nullOrEmptyDataNames ??= [];
            nullOrEmptyDataNames.Add("Id interno do Caixa do Mercado Pago");
        }

        if (nullOrEmptyDataNames != null && nullOrEmptyDataNames.Count > 0)
            throw new Exception($"Os seguintes dados não podem ser nulos/vazios: [{string.Join(',', nullOrEmptyDataNames)}]");
    }
}
