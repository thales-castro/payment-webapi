

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.WebApi.Dtos;
using PaymentSystem.WebApi.Enums;
using PaymentSystem.WebApi.Exceptions;
using PaymentSystem.WebApi.Services.PaymentDevices;
using PaymentSystem.WebApi.Services.Payments;
using PaymentSystem.WebApi.ViewModels;
using System.Net;

namespace PaymentSystem.WebApi.Controllers;

/// <summary>
/// Companies Controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentController : ControllerBase
{
    private IPaymentService _paymentService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<PaymentDevicesController> _logger;

    public PaymentController(IPaymentService paymentService, IHttpContextAccessor contextAccessor, ILogger<PaymentDevicesController> logger)
    {
        _paymentService = paymentService;
        _contextAccessor = contextAccessor;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentDto>>> GetPayments()
    {
        try
        {
            CheckPermissions();
            var paymentDtoList = await _paymentService.GetPayments();
            return StatusCode((int)HttpStatusCode.Created, paymentDtoList);
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
