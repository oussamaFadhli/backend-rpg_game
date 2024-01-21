using dotnet_recap.Dtos.Character;
using dotnet_recap.Models;
using dotnet_recap.Services.CharacterService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_recap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacter _characterService;

        public CharacterController(ICharacter characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("characters")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetAllCharacters()
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("characters/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetCharacterById(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost("characters/create")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var response = await _characterService.AddCharacter(newCharacter);
            if (response.Success == true) return CreatedAtAction(nameof(AddCharacter), response);
            return BadRequest(response);
        }

        [HttpPut("characters/edit")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var response = await _characterService.UpdateCharacter(updateCharacter);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("characters/delete/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
        {
            var response = await _characterService.DeleteCharacter(id);
            if (response.Data is null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("skill/create")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var response = await _characterService.AddCharacterSkill(newCharacterSkill);
            if (response.Success == true) return CreatedAtAction(nameof(AddCharacterSkill), response);
            return BadRequest(response);
        }
    }

}
