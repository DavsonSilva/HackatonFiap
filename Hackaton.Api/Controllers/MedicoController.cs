using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Requests.Medico;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Api.Controllers
{
    //[Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        public MedicoController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [Authorize(Roles = "Medico")]
        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            return Ok("Apenas médicos podem ver isso.");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicoResponse>>> GetAll()
        {
            var medicos = await _medicoService.GetAllAsync();
            return Ok(medicos);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MedicoResponse>> GetById(int id)
        {
            var medico = await _medicoService.GetByIdAsync(id);
            if (medico == null)
                return NotFound();

            return Ok(medico);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateMedicoRequest request)
        {
            await _medicoService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = request.CRM }, request);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateMedicoRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            await _medicoService.UpdateAsync(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _medicoService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MedicoResponse>>> Search([FromQuery] string query)
        {
            var medicos = await _medicoService.SearchAsync(query);
            return Ok(medicos);
        }
    }
}
