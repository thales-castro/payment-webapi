using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.WebApi.Dtos;
using PaymentSystem.WebApi.Dtos.Companies;
using PaymentSystem.WebApi.Enums;
using PaymentSystem.WebApi.Exceptions;
using PaymentSystem.WebApi.Services.Companies;
using PaymentSystem.WebApi.Services.PaymentDevices;
using PaymentSystem.WebApi.ViewModels;
using System.Net;

namespace PaymentSystem.WebApi.Controllers;

/// <summary>
/// Companies Controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentDevicesController : ControllerBase
{
    private readonly IPaymentDeviceService _service;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<PaymentDevicesController> _logger;

    public PaymentDevicesController(IPaymentDeviceService service, IHttpContextAccessor contextAccessor, ILogger<PaymentDevicesController> logger)
    {
        _service = service;
        _contextAccessor = contextAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Create a new PaymentDevice in Database.
    /// </summary>
    /// <param name="dto"> PaymentDevice parameters to be registered. </param>
    /// <returns> Action result with created PaymentDevice, or error message. </returns>
    [HttpPost]
    public ActionResult<PaymentDeviceDto> CreateAsync(PaymentDeviceDto dto)
    {
        try
        {
            CheckPermissions();
            return StatusCode((int)HttpStatusCode.Created, _service.Register(dto));
        }
        catch (ForbiddenResourceException e)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Get all companies registered in Database.
    /// </summary>
    /// <returns> Action result with List of Companies, or error message. </returns>
    [HttpGet]
    public async Task<ActionResult<List<PaymentDeviceViewModel>>> GetAllAsync()
    {
        try
        {
            CheckPermissions();
            return Ok(await _service.GetNotDeletedAsync());
        }
        catch (ForbiddenResourceException e)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Get an PaymentDevice information by its id.
    /// </summary>
    /// <param name="id"> Guid with PaymentDevice id. </param>
    /// <returns> Action result with PaymentDevice, or error message. </returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDeviceDto>> GetByIdAsync(string id)
    {
        try
        {
            var userClaims = _contextAccessor.HttpContext?.User.Claims;
            var userType = userClaims?.FirstOrDefault(claim => claim.Type == "UserType")?.Value;
            var companyId = userClaims?.FirstOrDefault(claim => claim.Type == "CompanyId")?.Value;

            if (userType != Enum.GetName(typeof(UserType), UserType.SystemAdmin)
                && companyId != id)
                throw new ForbiddenResourceException("User not allowed to complete this action.");

            return Ok(await _service.GetByIdAsync(id));
        }
        catch (ForbiddenResourceException e)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, e.Message);
        }
        catch (EntityNotFoundException e)
        {
            return StatusCode((int)HttpStatusCode.NotFound, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Update an PaymentDevice information in Database.
    /// </summary>
    /// <param name="entity"> PaymentDevice object to be updated. </param>
    /// <returns> Action result with updated PaymentDevice, or error message. </returns>
    [HttpPut]
    public async Task<ActionResult<PaymentDeviceDto>> UpdateAsync([FromBody] PaymentDeviceDto entity)
    {
        try
        {
            CheckPermissions();
            return Ok(await _service.UpdateAsync(entity));
        }
        catch (ForbiddenResourceException e)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Delete an PaymentDevice in Database.
    /// </summary>
    /// <param name="id"> Guid with PaymentDevice id. </param>
    /// <returns> Action result with deleted PaymentDevice, or error message. </returns>
    [HttpDelete("{id}")]
    public ActionResult<PaymentDeviceDto> DeleteAsync(string id)
    {
        try
        {
            CheckPermissions();
            return Ok(_service.Delete(id));
        }
        catch (EntityNotFoundException e)
        {
            return StatusCode((int)HttpStatusCode.NotFound, e.Message);
        }
        catch (ForbiddenResourceException e)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }

    private void CheckPermissions()
    {
        var userType = _contextAccessor.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == "UserType")?.Value;
        if (userType != Enum.GetName(typeof(UserType), UserType.SystemAdmin))
        {
            _logger.LogWarning($"User [{_contextAccessor.HttpContext?.User.Identity?.Name}] is not allowed to complete this action. " +
                $"Only admin users are allowed.");
            throw new ForbiddenResourceException("Permission denied. Resource only available for Administrators.");
        }
    }
}
