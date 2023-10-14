using VebTech.Domain.Models;
using VebTech.Domain.Models.DTO;
using VebTech.Domain.Models.Parameters;

namespace VebTech.Infrastructure.Repositories.Abstract;

public interface IUserRepository
{
    IEnumerable<User>? GetUsers(PaginationParameters paginationParameters,
        SortParameters sortParameters, FilterParameters filterParameters);
    Task<User?> GetUser(int id);
    Task<User?> CreateUser(UserDto userDto);
    Task<User?> UpdateUser(int id, UserDto userDto);
    Task<User?> DeleteUser(int id);
    Task<bool> IsEmailExist(string email);
    Task<Role?> CreateRole(RoleDto roleDto);
    IEnumerable<User> SortUsers(IEnumerable<User> users, SortParameters sortParameters);
    IEnumerable<User>? FilterUsers(IEnumerable<User> users, FilterParameters filterParameters);
}