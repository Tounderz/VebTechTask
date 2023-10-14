using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VebTech.Application.Services.Interfaces;
using VebTech.CustomException;
using VebTech.Domain.Models;
using VebTech.Domain.Models.DTO;
using VebTech.Domain.Models.EnumModels;
using VebTech.Domain.Models.Parameters;
using VebTech.Validators.Interfaces;

namespace VebTech.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidate _validate;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService,
        ILogger<UsersController> logger,
        IValidate validate)
    {
        _userService = userService;
        _logger = logger;
        _validate = validate;
    }

    [ProducesResponseType(typeof(ICollection<User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public IActionResult Get(
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] SortParameters sortParameters,
        [FromQuery] FilterParameters filterParameters)
    {
        try
        {
            if (!string.IsNullOrEmpty(sortParameters.OrderBy)
                && sortParameters.SortOrder != SortDirection.Ascending
                && sortParameters.SortOrder != SortDirection.Descending)
            {
                return BadRequest(new { message = "Not valid sort order" });
            }

            var users = _userService.GetUsers(paginationParameters, sortParameters, filterParameters);
            return users == null
                ? NotFound(new { message = "The list of users is empty" })
                : Ok(new { users });
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

    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var user = await _userService.GetUser(id);
            return user == null ?
                NotFound(new { message = "User not found" })
                : Ok(new { user });
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

    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] UserDto userDto)
    {
        try
        {
            await _validate.ValidateCreate(userDto);
            var user = await _userService.CreateUser(userDto);
            return user == null ?
                BadRequest(new { message = "Failed to create a user" })
                : Ok(new { user });
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

    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromForm] UserDto userDto)
    {
        try
        {
            await _validate.ValidateUpdate(userDto);
            var user = await _userService.UpdateUser(id, userDto);
            return user == null ?
                NotFound(new { message = "User not found" })
                : Ok(new { user });
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var user = await _userService.DeleteUser(id);
            return user == null
                ? NotFound(new { message = "User not found" })
                : Ok(new { message = "successfully" });
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

    [ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("role")]
    public async Task<IActionResult> CreateRole([FromForm] RoleDto roleDto)
    {
        try
        {
            var role = await _userService.CreateRole(roleDto);
            return role == null
                ? BadRequest(new { message = "Failed to create a role" })
                : Ok(new { role = await _userService.CreateRole(roleDto) });
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