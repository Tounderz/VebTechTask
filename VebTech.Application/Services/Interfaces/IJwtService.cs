namespace VebTech.Application.Services.Interfaces;

public interface IJwtService
{
    public string GenerateJwt(string email);
}