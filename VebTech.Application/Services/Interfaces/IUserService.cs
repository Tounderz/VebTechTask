using VebTech.Domain.Models;
using VebTech.Domain.Models.DTO;
using VebTech.Domain.Models.Parameters;

namespace VebTech.Application.Services.Interfaces;

public interface IUserService
{
    IEnumerable<User>? GetUsers(PaginationParameters paginationParameters,
         SortParameters sortParameters, FilterParameters filterParameters);
    Task<User?> GetUser(int id);
    Task<User?> CreateUser(UserDto userDto);
    Task<User?> UpdateUser(int id, UserDto userDto);
    Task<User?> DeleteUser(int id);
    Task<bool> IsEmailExist(string email);
    Task<Role?> CreateRole(RoleDto roleDto);
}