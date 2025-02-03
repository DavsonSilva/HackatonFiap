using Hackaton.Domain.Requests.Consulta;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;
using Microsoft.AspNetCore.Authorization;
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

        //[Authorize(Roles = "Paciente")]
        [HttpPost("agendar")]
        public async Task<IActionResult> AgendarConsulta([FromBody] CreateConsultaRequest request)
        {
            var consulta = await _consultaService.AgendarConsultaAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = consulta.Id }, consulta);
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
            if (consultas == null)
            {
                return NotFound(new { Message = "Nenhuma consulta encontrada para este Pasciente." });
            }
            return Ok(consultas);
        }

        [HttpGet("historico/medico/{medicoId}")]
        public async Task<ActionResult<MedicoHistoricoResponse>> GetHistoricoMedico(int medicoId)
        {
            var historico = await _consultaService.GetHistoricoMedicoAsync(medicoId);

            if (historico == null)
            {
                return NotFound(new { Message = "Nenhuma consulta encontrada para este médico." });
            }

            return Ok(historico);
        }

    }
}
