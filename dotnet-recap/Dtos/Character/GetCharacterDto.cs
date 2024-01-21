using dotnet_recap.Dtos.Skill;
using dotnet_recap.Dtos.Weapon;
using dotnet_recap.Models;

namespace dotnet_recap.Dtos.Character
{
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string? Name { get; set; } = "Oussama";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 5;
        public int Intelligence { get; set; } = 80;
        public RpgClass RpgClass { get; set; } = RpgClass.Knight;
        public GetWeaponDto? Weapon { get; set; }
        public List<GetSkillDto>? Skills { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}
