using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.WebApi.Dtos.Companies;
using PaymentSystem.WebApi.Enums;
using PaymentSystem.WebApi.Exceptions;
using PaymentSystem.WebApi.Services.Companies;
using System.Net;

namespace PaymentSystem.WebApi.Controllers;

/// <summary>
/// Companies Controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _service;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<CompaniesController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="service"> Company Service </param>
    /// <param name="contextAccessor"> Context Request </param>
    /// <param name="logger"> Logger </param>
    public CompaniesController(ICompanyService service, IHttpContextAccessor contextAccessor, ILogger<CompaniesController> logger)
    {
        _service = service;
        _contextAccessor = contextAccessor;
        _logger = logger;
    }

    /// <summary>
    /// Create a new Company in Database.
    /// </summary>
    /// <param name="dto"> Company parameters to be registered. </param>
    /// <returns> Action result with created Company, or error message. </returns>
    [HttpPost]
    public async Task<ActionResult<CompanyDto>> CreateAsync(CompanyDto dto)
    {
        try
        {
            CheckPermissions();
            CompanyDto newDto = await _service.Register(dto);
            return StatusCode((int)HttpStatusCode.Created, newDto);
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
    public async Task<ActionResult<List<CompanyDto>>> GetAllAsync()
    {
        try
        {
            CheckPermissions();
            return Ok(await _service.GetNotDeleted());
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
    /// Get an Company information by its id.
    /// </summary>
    /// <param name="id"> Guid with Company id. </param>
    /// <returns> Action result with Company, or error message. </returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyDto>> GetByIdAsync(string id)
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
    /// Update an Company information in Database.
    /// </summary>
    /// <param name="entity"> Company object to be updated. </param>
    /// <returns> Action result with updated Company, or error message. </returns>
    [HttpPut]
    public async Task<ActionResult<CompanyDto>> UpdateAsync([FromBody] CompanyDto entity)
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
    /// Delete an Company in Database.
    /// </summary>
    /// <param name="id"> Guid with Company id. </param>
    /// <returns> Action result with deleted Company, or error message. </returns>
    [HttpDelete("{id}")]
    public ActionResult<CompanyDto> DeleteAsync(string id)
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
