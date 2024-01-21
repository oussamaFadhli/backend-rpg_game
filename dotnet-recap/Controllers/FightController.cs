using dotnet_recap.Dtos.Fight;
using dotnet_recap.Models;
using dotnet_recap.Services.Fight;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_recap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FightController : ControllerBase
    {
        private readonly IFight _fightService;
        public FightController(IFight fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("weapon-fight")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto request)
        {
            var response = await _fightService.WeaponAttack(request);

            if (response.Success == true)
            {
                return CreatedAtAction(nameof(WeaponAttack), response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("skill-fight")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto request)
        {
            var response = await _fightService.SkillAttack(request);

            if (response.Success == true)
            {
                return CreatedAtAction(nameof(SkillAttack), response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("random-fight")]
        public async Task<ActionResult<ServiceResponse<FightResultDto>>> Fight(FightRequestDto request)
        {
            var response = await _fightService.Fight(request);

            if (response.Success == true)
            {
                return CreatedAtAction(nameof(Fight), response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("high-score")]
        public async Task<ActionResult<ServiceResponse<HighScoreDto>>> GetHighScore()
        {
            return Ok(_fightService.GetHighScore());
        }

    }
}
