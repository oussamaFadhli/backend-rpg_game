using AutoMapper;
using dotnet_recap.Data;
using dotnet_recap.Dtos.Character;
using dotnet_recap.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;

namespace dotnet_recap.Services.CharacterService
{
    public class CharacterService : ICharacter
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        

        public CharacterService(IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _dataContext = dataContext;
            
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var character = _mapper.Map<Character>(newCharacter);

                _dataContext.Add(character);
                await _dataContext.SaveChangesAsync();

                serviceResponse.Data = await _dataContext.Characters
                    .Select(c => _mapper.Map<GetCharacterDto>(c))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = $"Error adding character: {ex.Message}";
            }

            return serviceResponse;
        }



        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == id);
                if (character is null)
                    throw new Exception($"Character with Id {id} not found");
                _dataContext.Characters.Remove(character);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data =
                    await _dataContext.Characters
                        .Select(c => _mapper.Map<GetCharacterDto>(c))
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _dataContext.Characters
                .Include(c => c.Skills)
                .Include(c => c.Weapon)
                .ToListAsync();
            serviceResponse.Data =
                dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;

        }


        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _dataContext.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data =
                _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;

        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var dbCharacter = await _dataContext.Characters
                    .FirstOrDefaultAsync(c=>c.Id == updateCharacter.Id);
                if (dbCharacter is null)
                {
                    throw new Exception($"Character with Id {updateCharacter.Id} not found");
                }
                dbCharacter.Name = updateCharacter.Name;
                dbCharacter.HitPoints = updateCharacter.HitPoints;
                dbCharacter.Strength = updateCharacter.Strength;
                dbCharacter.Defense = updateCharacter.Defense;
                dbCharacter.Intelligence = updateCharacter.Intelligence;
                dbCharacter.RpgClass = updateCharacter.RpgClass;
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data =
                    _mapper.Map<GetCharacterDto>(dbCharacter);
                
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
            return serviceResponse;
        }


        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto> ();
            try
            {
                var character = await _dataContext.Characters
                    .Include(c => c.Skills)
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId);
                if (character is null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found";
                    return serviceResponse;
                }
                var skill = await _dataContext.Skills
                    .FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if (skill is null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Skill not found";
                    return serviceResponse;
                }
                character.Skills!.Add(skill);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data =
                    _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success= false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
            return serviceResponse;
        }
    }
}
