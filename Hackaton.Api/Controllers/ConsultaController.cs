using Hackaton.Domain.Requests.Consulta;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConsultaController : ControllerBase
    {
        private readonly IConsultaService _consultaService;

        public ConsultaController(IConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsultaResponse>>> GetAll()
        {
            var consultas = await _consultaService.GetAllAsync();
            return Ok(consultas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConsultaResponse>> GetById(int id)
        {
            var consulta = await _consultaService.GetByIdAsync(id);
            if (consulta == null)
                return NotFound();

            return Ok(consulta);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateConsultaRequest request)
        {
            await _consultaService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = request.PacienteId }, request);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateConsultaRequest request)
        {
            if (id != request.Id)
                return BadRequest();

            await _consultaService.UpdateAsync(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _consultaService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("cancelar")]
        public async Task<ActionResult> CancelarConsulta([FromBody] CancelConsultaRequest request)
        {
            await _consultaService.CancelarConsultaAsync(request);
            return Ok(new { Message = "Consulta cancelada com sucesso!" });
        }

        [HttpGet("historico/paciente/{pacienteId}")]
        public async Task<ActionResult<IEnumerable<ConsultaResponse>>> GetHistoricoPaciente(int pacienteId)
        {
            var consultas = await _consultaService.GetHistoricoPacienteAsync(pacienteId);
            return Ok(consultas);
        }

        [HttpGet("historico/medico/{medicoId}")]
        public async Task<ActionResult<IEnumerable<ConsultaResponse>>> GetHistoricoMedico(int medicoId)
        {
            var consultas = await _consultaService.GetHistoricoMedicoAsync(medicoId);
            return Ok(consultas);
        }

    }
}
