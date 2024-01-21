using dotnet_recap.Models;

namespace dotnet_recap.Dtos.Character
{
    public class UpdateCharacterDto
    {
        public int Id { get; set; }
        public string? Name { get; set; } = "Oussama";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 5;
        public int Intelligence { get; set; } = 80;
        public RpgClass RpgClass { get; set; } = RpgClass.Knight;
    }
}
