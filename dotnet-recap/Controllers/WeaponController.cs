using dotnet_recap.Dtos.Character;
using dotnet_recap.Dtos.Weapon;
using dotnet_recap.Models;
using dotnet_recap.Services.Weapon;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_recap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeapon _weaponService;

        public WeaponController(IWeapon weaponService)
        {
            _weaponService = weaponService;
        }
        [HttpPost("create")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(await _weaponService.AddWeapon(newWeapon));
        }
    }
}
