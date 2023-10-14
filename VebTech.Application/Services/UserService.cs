using VebTech.Application.Services.Interfaces;
using VebTech.Domain.Models;
using VebTech.Domain.Models.DTO;
using VebTech.Domain.Models.Parameters;
using VebTech.Infrastructure.Repositories.Abstract;

namespace VebTech.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IEnumerable<User>? GetUsers(PaginationParameters paginationParameters,
        SortParameters sortParameters, FilterParameters filterParameters)
    {
        return _userRepository.GetUsers(paginationParameters, sortParameters, filterParameters);
    }

    public async Task<User?> GetUser(int id) => await _userRepository.GetUser(id);

    public async Task<User?> CreateUser(UserDto userDto)
    {
        return await _userRepository.CreateUser(userDto);
    }

    public async Task<User?> UpdateUser(int id, UserDto userDto)
    {
        return await _userRepository.UpdateUser(id, userDto);
    }

    public async Task<User?> DeleteUser(int id)
    {
        return await _userRepository.DeleteUser(id);
    }

    public async Task<bool> IsEmailExist(string email)
    {
        return await _userRepository.IsEmailExist(email);
    }

    public async Task<Role?> CreateRole(RoleDto roleDto)
    {
        return await _userRepository.CreateRole(roleDto);
    }
}