using Microsoft.AspNetCore.Mvc;
using VebTech.Application.Services.Interfaces;
using VebTech.CustomException;
using VebTech.Domain.Models.DTO;
using VebTech.Validators.Interfaces;

namespace VebTech.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IValidate _validate;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IAuthenticationService authenticationService,
        ILogger<AuthenticationController> logger,
        IJwtService jwtService,
        IValidate validate)
    {
        _authenticationService = authenticationService;
        _logger = logger;
        _jwtService = jwtService;
        _validate = validate;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("/signIn")]
    public async Task<IActionResult> SignIn([FromForm] AdminDto adminDto)
    {
        try
        {
            _validate.ValidateAdmin(adminDto);
            var admin = await _authenticationService.SignIn(adminDto);
            if (admin == null)
            {
                return NotFound(new { message = "Invalid email or password" });
            }

            var encodedJwt = _jwtService.GenerateJwt(admin.Email);
            var response = new
            {
                access_token = encodedJwt,
                username = admin.Email
            };

            return Ok(response);
        }
        catch (HttpResponseException ex)
        {
            _logger.Log(LogLevel.Error, ex, ex.Message, ex.StatusCode);
            return BadRequest(new { message = ex.Value });
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, ex.Message, ex.HResult);
            return BadRequest(new { message = "Error" });
        }
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("/signUp")]
    public async Task<IActionResult> SignUp([FromForm] AdminDto adminDto)
    {
        try
        {
            _validate.ValidateAdmin(adminDto);
            if (await _authenticationService.IsExistEmail(adminDto.Email!))
            {
                return BadRequest(new { message = "Admin with this email exist" });
            }

            var admin = await _authenticationService.SignUp(adminDto);
            if (admin == null)
            {
                return BadRequest(new { message = "Error" });
            }

            return Ok(new { message = "successfully" });
        }
        catch (HttpResponseException ex)
        {
            _logger.Log(LogLevel.Error, ex, ex.Message, ex.StatusCode);
            return BadRequest(new { message = ex.Value });
        }
        catch (Exception ex)
        {
            _logger.Log(LogLevel.Error, ex, ex.Message, ex.HResult);
            return BadRequest(new { message = "Error" });
        }
    }
}