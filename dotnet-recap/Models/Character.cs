namespace dotnet_recap.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string? Name { get; set; } = "Oussama";
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 5;
        public int Intelligence { get; set; } = 10;
        public RpgClass RpgClass { get; set; } = RpgClass.Knight;
        public User? User { get; set; }
        public Weapon? Weapon { get; set; }
        public List<Skill>? Skills { get; set; }
        public int Fights { get; set; }
        public int Victories {  get; set; }
        public int Defeats { get; set; }

    }
}
