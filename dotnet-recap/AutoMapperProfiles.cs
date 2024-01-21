using AutoMapper;
using dotnet_recap.Dtos.Character;
using dotnet_recap.Dtos.Fight;
using dotnet_recap.Dtos.Skill;
using dotnet_recap.Dtos.Weapon;
using dotnet_recap.Models;

namespace dotnet_recap
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<Weapon, GetWeaponDto>();
            CreateMap<Skill, GetSkillDto>();
            CreateMap<Character, HighScoreDto>();
        }
    }
}
