using VebTech.Domain.Models;
using VebTech.Domain.Models.DTO;

namespace VebTech.Infrastructure.Repositories.Abstract;

public interface IAuthenticationRepository
{
    Task<Admin?> SignIn(AdminDto adminDto);
    Task<Admin?> SignUp(AdminDto adminDto);
    Task<bool> IsExistEmail(string email);
}