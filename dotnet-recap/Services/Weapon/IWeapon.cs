using dotnet_recap.Dtos.Character;
using dotnet_recap.Dtos.Weapon;
using dotnet_recap.Models;

namespace dotnet_recap.Services.Weapon
{
    public interface IWeapon
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}
