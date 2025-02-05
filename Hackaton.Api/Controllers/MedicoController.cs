using Hackaton.Domain.Requests.Agenda;
using Hackaton.Domain.Requests.Medico;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        public MedicoController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicoResponse>>> GetAll()
        {
            var medicos = await _medicoService.GetAllAsync();
            return Ok(medicos);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Medico")]
        public async Task<ActionResult<MedicoResponse>> GetById(int id)
        {
            var medico = await _medicoService.GetByIdAsync(id);
            if (medico == null)
                return NotFound();

            return Ok(medico);
        }

        [HttpPost]
        public async Task<ActionResult<MedicoResponse>> Create([FromBody] CreateMedicoRequest request)
        {
            var medicoCriado = await _medicoService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = medicoCriado.Id }, medicoCriado);
        }

        [HttpPut("atualizarMedico")]
        public async Task<ActionResult> Update([FromBody] UpdateMedicoRequest request)
        {
            if (request.Id != request.Id)
                return BadRequest();

            await _medicoService.UpdateAsync(request);
            return NoContent();
        }

        [HttpDelete("excluirMedico/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _medicoService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/agenda")]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> AddAgenda(int id, [FromBody] List<CreateAgendaRequest> agendaRequests)
        {
            await _medicoService.AddAgendaAsync(id, agendaRequests);
            return NoContent();
        }

        [HttpPut("{id}/agenda/{agendaId}")]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> EditarAgenda(int id, int agendaId, [FromBody] UpdateAgendaRequest request)
        {
            await _medicoService.EditarAgendaAsync(id, agendaId, request);
            return Ok(new { message = "Horário atualizado com sucesso!" });
        }

        [HttpDelete("{medicoId}/agenda/{agendaId}")]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> ExcluirAgenda(int medicoId, int agendaId)
        {
            await _medicoService.ExcluirAgendaAsync(medicoId, agendaId);
            return Ok(new { message = "Horário removido com sucesso!" });
        }
    }
}
