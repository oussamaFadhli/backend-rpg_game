using dotnet_recap.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_recap.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> option) : base(option) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = 1,Name = "Fireball",Damage = 30},
                new Skill { Id = 2,Name ="Frenzy",Damage = 20},
                new Skill { Id = 3 , Name = "Bilzzard",Damage = 50}
                );
        }


        public DbSet<Character> Characters => Set<Character>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<Weapon> Weapons => Set<Weapon>();
    }
}
