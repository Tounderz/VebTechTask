﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VebTech.Domain.Models;
using VebTech.Domain.Models.DTO;
using VebTech.Domain.Models.EnumModels;
using VebTech.Domain.Models.Parameters;
using VebTech.Infrastructure.Context;
using VebTech.Infrastructure.Repositories.Abstract;

namespace VebTech.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly Mapper _mapper;
    public UserRepository(AppDbContext context, Mapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public IEnumerable<User>? GetUsers(PaginationParameters paginationParameters,
        SortParameters sortParameters, FilterParameters filterParameters)
    {
        IEnumerable<User>? users = _context.Users
            .Include(user => user.Roles)
            .AsQueryable();

        if (!users.Any())
        {
            return null;
        }

        if (filterParameters != null)
        {
            users = FilterUsers(users, filterParameters);
        }

        if (users != null && !string.IsNullOrEmpty(sortParameters.OrderBy))
        {
            users = SortUsers(users, sortParameters);
        }

        return users?.Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize);
    }

    public async Task<User?> GetUser(int id)
    {
        var user = await _context.Users
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(user => user.Id == id);
        if (user == null)
        {
            return null;
        }

        return user;
    }

    public async Task<User?> CreateUser(UserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateUser(int id, UserDto userDto)
    {
        var oldUser = await GetUser(id);
        if (oldUser == null)
        {
            return null;
        }

        _mapper.Map(userDto, oldUser);
        await _context.SaveChangesAsync();
        return oldUser;
    }

    public async Task<User?> DeleteUser(int id)
    {
        var user = await GetUser(id);
        if (user == null)
        {
            return null;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<Role?> CreateRole(RoleDto roleDto)
    {
        var user = await GetUser(int.Parse(roleDto.UserId!));
        if (user == null)
        {
            return null;
        }

        var role = _mapper.Map<Role>(roleDto);

        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<bool> IsEmailExist(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email == email) != null;
    }

    public IEnumerable<User> SortUsers(IEnumerable<User> users, SortParameters sortParameters)
    {
        return sortParameters.OrderBy.ToLower() switch
        {
            "id" => sortParameters.SortOrder == SortDirection.Ascending
                                ? users.OrderBy(i => i.Id)
                                : users.OrderByDescending(i => i.Id),
            "name" => sortParameters.SortOrder == SortDirection.Ascending
                                ? users.OrderBy(i => i.Name)
                                : users.OrderByDescending(i => i.Name),
            "age" => sortParameters.SortOrder == SortDirection.Ascending
                                ? users.OrderBy(i => i.Age)
                                : users.OrderByDescending(i => i.Age),
            "email" => sortParameters.SortOrder == SortDirection.Ascending
                                ? users.OrderBy(i => i.Email)
                                : users.OrderByDescending(i => i.Email),
            _ => throw new ArgumentException("Not valid sort field"),
        };
    }

    public IEnumerable<User>? FilterUsers(IEnumerable<User> users, FilterParameters filterParameters)
    {
        if (!string.IsNullOrEmpty(filterParameters.SearchQuery))
        {
            users = users.Where(user => user.Name.Contains(filterParameters.SearchQuery)
            || user.Email.Contains(filterParameters.SearchQuery)
            || user.Age.ToString().Contains(filterParameters.SearchQuery)
            || user.Roles != null && user.Roles.Any(role => role.Name.Contains(filterParameters.SearchQuery, StringComparison.OrdinalIgnoreCase)));
        }

        if (!string.IsNullOrEmpty(filterParameters.Name))
        {
            users = users.Where(user => user.Name.Contains(filterParameters.Name, StringComparison.OrdinalIgnoreCase));
        }

        if (filterParameters.Age != null)
        {
            users = users.Where(user => user.Age == filterParameters.Age);
        }

        if (!string.IsNullOrEmpty(filterParameters.Email))
        {
            users = users.Where(user => user.Email.Contains(filterParameters.Email, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(filterParameters.RoleName))
        {
            users = users.Where(user => user.Roles != null && user.Roles.Any(role => role.Name.Contains(filterParameters.RoleName, StringComparison.OrdinalIgnoreCase)));
        }

        if (!users.Any())
        {
            return null;
        }

        return users;
    }
}
