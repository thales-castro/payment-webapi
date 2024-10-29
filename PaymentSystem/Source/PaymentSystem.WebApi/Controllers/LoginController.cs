using Microsoft.AspNetCore.Mvc;
using PaymentSystem.WebApi.Dtos;
using PaymentSystem.WebApi.Exceptions;
using PaymentSystem.WebApi.Services.Auth;
using System.Net;

namespace PaymentSystem.WebApi.Controllers;

/// <summary>
/// Login Controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IAuthService _service;

    /// <summary>
    /// Login Controller Constructor
    /// </summary>
    /// <param name="service"> Service to Authorize and Authenticate users </param>
    public LoginController(IAuthService service) =>
        _service = service;

    /// <summary>
    /// Login to system.
    /// </summary>
    /// <param name="dto"> DTO with Username & Password </param>
    /// <returns> Http Response + JWT Bearer token (if success). </returns>
    [HttpPost]
    public async Task<ActionResult<string>> Login([FromBody] LoginDto dto)
    {
        try
        {
            return Ok(await _service.Login(dto.Username, dto.Password));
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
    }
}
