using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VebTech.Domain.Models;
using VebTech.Domain.Models.DTO;
using VebTech.Infrastructure.Context;
using VebTech.Infrastructure.Repositories.Abstract;

namespace VebTech.Infrastructure.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly AppDbContext _context;
    private readonly Mapper _mapper;

    public AuthenticationRepository(AppDbContext context, Mapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Admin?> SignIn(AdminDto adminDto)
    {
        var admin = await _context.Admins.FirstOrDefaultAsync(user => user.Email == adminDto.Email);
        return admin == null || !BCrypt.Net.BCrypt.Verify(adminDto.Password, admin.Password) ? null : admin;
    }

    public async Task<Admin?> SignUp(AdminDto adminDto)
    {
        var admin = _mapper.Map<Admin>(adminDto);

        await _context.Admins.AddAsync(admin);
        await _context.SaveChangesAsync();
        return admin;
    }

    public async Task<bool> IsExistEmail(string email)
    {
        return await _context.Admins.FirstOrDefaultAsync(admin => admin.Email == email) != null;
    }
}