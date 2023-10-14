using VebTech.Domain.Models.DTO;

namespace VebTech.Validators.Interfaces;

public interface IValidate
{
    Task ValidateCreate(UserDto userDto);
    Task ValidateUpdate(UserDto userDto);
    void ValidateAdmin(AdminDto adminDto);
    void ValidateCreateRole(RoleDto roleDto);
}
