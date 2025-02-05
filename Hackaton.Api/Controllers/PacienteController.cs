using Hackaton.Domain.Requests.Paciente;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacienteController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PacienteResponse>>> GetAll()
        {
            var pacientes = await _pacienteService.GetAllAsync();
            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Paciente")]
        public async Task<ActionResult<PacienteResponse>> GetById(int id)
        {
            var paciente = await _pacienteService.GetByIdAsync(id);
            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }

        [HttpPost]
        public async Task<ActionResult<PacienteResponse>> Create([FromBody] CreatePacienteRequest request)
        {
            var pacienteCriado = await _pacienteService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = pacienteCriado.Id }, pacienteCriado);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdatePacienteRequest request)
        {
            if (request.Id != request.Id)
                return BadRequest();

            await _pacienteService.UpdateAsync(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _pacienteService.DeleteAsync(id);
            return NoContent();
        }
    }
}
