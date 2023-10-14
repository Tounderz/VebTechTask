using VebTech.Application.Services.Interfaces;
using VebTech.Domain.Models;
using VebTech.Domain.Models.DTO;
using VebTech.Infrastructure.Repositories.Abstract;

namespace VebTech.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationRepository _authenticationRepository;

    public AuthenticationService(IAuthenticationRepository authenticationRepository)
    {
        _authenticationRepository = authenticationRepository;
    }

    public async Task<Admin?> SignIn(AdminDto adminDto)
    {
        return await _authenticationRepository.SignIn(adminDto);
    }

    public async Task<Admin?> SignUp(AdminDto adminDto)
    {
        return await _authenticationRepository.SignUp(adminDto);
    }

    public async Task<bool> IsExistEmail(string email)
    {
        return await _authenticationRepository.IsExistEmail(email);
    }
}