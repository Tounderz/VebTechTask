using VebTech.Domain.Models;
using VebTech.Domain.Models.DTO;

namespace VebTech.Application.Services.Interfaces;

public interface IAuthenticationService
{
    Task<Admin?> SignIn(AdminDto adminDto);
    Task<Admin?> SignUp(AdminDto adminDto);
    Task<bool> IsExistEmail(string email);
}