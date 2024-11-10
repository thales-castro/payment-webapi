using PaymentSystem.WebApi.Database.Repositories;
using PaymentSystem.WebApi.Dtos;
using PaymentSystem.WebApi.Exceptions;
using PaymentSystem.WebApi.Mappers;
using PaymentSystem.WebApi.Services.Companies;
using PaymentSystem.WebApi.ViewModels;

namespace PaymentSystem.WebApi.Services.PaymentDevices;

public class PaymentDeviceService : IPaymentDeviceService
{
    private readonly IPaymentDeviceRepository _repository;
    private readonly ICompanyService _companyService;

    public PaymentDeviceService(IPaymentDeviceRepository repository, ICompanyService companyService)
    {
        _repository = repository;
        _companyService = companyService;
    }

    public PaymentDeviceDto Register(PaymentDeviceDto dto)
    {
        var entity = PaymentDeviceMapper.GetEntityFromDto(dto);
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
        if (dto.Id == null)
            throw new Exception("Id can't be null.");

        var entity = await _repository.GetByIdAsync(dto.Id.ToString()) ??
            throw new Exception($"Could not find Company with id {dto.Id}");
        entity.UpdateEntityFromDto(dto);
        _repository.Update(entity);
        return PaymentDeviceMapper.GetDtoFromEntity(entity);
    }
    public PaymentDeviceDto Delete(string id)
    {
        var entity = _repository.Delete(id);

        var dto = PaymentDeviceMapper.GetDtoFromEntity(entity) ??
            throw new EntityNotFoundException($"Company with id [{id}] not found.");
        return dto;
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
}
