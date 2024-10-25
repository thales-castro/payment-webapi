using Microsoft.AspNetCore.Mvc;
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
    /// <param name="username"> Username to login. </param>
    /// <param name="password"> Password to login. </param>
    /// <returns> Http Response + JWT Bearer token (if success). </returns>
    [HttpGet]
    public async Task<ActionResult<string>> Login(string username, string password)
    {
        try
        {
            return Ok(await _service.Login(username, password));
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
