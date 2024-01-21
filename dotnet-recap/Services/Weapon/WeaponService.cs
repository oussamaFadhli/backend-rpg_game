using AutoMapper;
using dotnet_recap.Data;
using dotnet_recap.Dtos.Character;
using dotnet_recap.Dtos.Weapon;
using dotnet_recap.Models;
using Microsoft.EntityFrameworkCore;
using dotnet_recap;

namespace dotnet_recap.Services.Weapon
{
    public class WeaponService : IWeapon
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;


        public WeaponService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }



        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _dataContext.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId);

                if (character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found.";
                    return response;
                }

                var weapon = new dotnet_recap.Models.Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };

                _dataContext.Weapons.Add(weapon);
                await _dataContext.SaveChangesAsync();

                character = await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId);

                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}