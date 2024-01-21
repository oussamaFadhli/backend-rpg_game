using AutoMapper;
using dotnet_recap.Data;
using dotnet_recap.Dtos.Fight;
using dotnet_recap.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace dotnet_recap.Services.Fight
{
    public class FightService : IFight
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
            var response = new ServiceResponse<FightResultDto>
            {
                Data = new FightResultDto()
            };

            try
            {
                var characters = await _context.Characters
                    .Include(c => c.Skills)
                    .Include(c => c.Weapon)
                    .Where(c => request.CharacterIds.Contains(c.Id))
                    .ToListAsync();
                bool defeated = false;
                while (!defeated)
                {
                    foreach(var attacker in characters)
                    {
                        var oppenents = characters.Where(c=>c.Id != attacker.Id).ToList();
                        var oppenent = oppenents[new Random().Next(oppenents.Count)];

                        int damage = 0;
                        string? attackUsed = string.Empty;
                        
                        bool useWeapon = new Random().Next(2) == 0;
                        if (useWeapon && attacker.Weapon is not null)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, oppenent);
                        }
                        else if (!useWeapon && attacker.Skills is not null)
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, oppenent,skill);
                        }
                        else
                        {
                            response.Data.Log.Add($"{attacker.Name} wasn't able to attack");
                            continue;
                        }
                        response.Data.Log
                            .Add($"{attacker.Name} attacks {oppenent.Name} using {attackUsed} with {(damage >=0 ? damage : 0)} damage");
                        if (oppenent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            oppenent.Defeats++;
                            response.Data.Log.Add($"{oppenent.Name} has been defeated");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} HP left");
                            break;
                        }
                    }
                }
                characters.ForEach(c =>
                {
                    c.Fights++;
                    c.HitPoints = 100;
                });
                await _context.SaveChangesAsync();

                
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message =ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters
                    .Include(c=>c.Skills)
                    .FirstOrDefaultAsync(c=>c.Id == request.OpponentId);


                if (attacker is null || opponent is null || attacker.Skills is null)
                {
                    throw new Exception($"Something fishy is going on here ...");
                }

                var skill = attacker.Skills.FirstOrDefault(s=>s.Id == request.SkillId);
                if (skill is null)
                {
                    response.Success=false;
                    response.Message = $"{attacker.Name} doesn't know that skill";
                    return response;
                }
                int damage = DoSkillAttack(attacker, opponent, skill);
                if (opponent.HitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated";

                }

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage

                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }


        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);
                if (attacker is null || opponent is null || attacker.Weapon is null)
                {
                    throw new Exception($"Something is fishy is going on here");
                }
                int damage = DoWeaponAttack(attacker, opponent);
                if (opponent.HitPoints <= 0)
                {
                    response.Message = ($"{opponent.Name} has been defeated");
                }
                await _context.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }


        public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
        {
            var characters = await _context.Characters
                .Where(c=>c.Fights>=0)
                .OrderByDescending(c=>c.Victories)
                .ThenBy(c=>c.Defeats)
                .ToListAsync();

            var response = new ServiceResponse<List<HighScoreDto>>()
            {
                Data = characters.Select(c=>_mapper.Map<HighScoreDto>(c)).ToList()
            };
            return response;
        }

        private static int DoWeaponAttack(Character attacker,Character oppenent)
        {
            if (attacker.Weapon is null)
               throw new Exception($"Attacker has no weapon");
            int damage = attacker.Weapon.Damage+(new Random().Next(attacker.Strength));
            damage -= new Random().Next(oppenent.Defeats);
            if (damage>0) oppenent.HitPoints -= damage;
            return damage;
            
        }

        private static int DoSkillAttack(Character attacker , Character oppenent,Skill skill)
        {
            int damage = (int) skill.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(oppenent.Defeats);

            if (damage > 0)
                oppenent.HitPoints -= damage;
            return damage;
        }
    }
}
