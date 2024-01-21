using dotnet_recap.Dtos.Fight;
using dotnet_recap.Models;

namespace dotnet_recap.Services.Fight
{
    public interface IFight
    {
        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
        Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request);
        Task<ServiceResponse<List<HighScoreDto>>> GetHighScore();

    }
}
