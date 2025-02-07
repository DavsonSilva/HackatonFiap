using Hackaton.Domain.Requests.Base;
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
        public async Task<ActionResult<ConsultaListaResponse>> GetAll([FromQuery] BaseConsultaPaginada request)
        {
            var consultas = await _consultaService.GetAllAsync(request);

            if (consultas == null || !consultas.Medicos.Any())
            {
                return NotFound(new { Message = "Nenhuma consulta encontrada." });
            }

            return Ok(consultas);
        }

        [Authorize(Roles = "Paciente")]
        [HttpPost("agendar")]
        public async Task<IActionResult> AgendarConsulta([FromBody] CreateConsultaRequest request)
        {
            var consulta = await _consultaService.AgendarConsultaAsync(request);
            return Ok(new { Message = "Consulta agendada com sucesso!", Consulta = consulta });
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

        [HttpGet("pendentes/{medicoId}")]
        [Authorize(Roles = "Medico")]
        public async Task<ActionResult<IEnumerable<ConsultaResponse>>> GetPendentes(int medicoId)
        {
            var consultasPendentes = await _consultaService.GetPendentesAsync(medicoId);
            return Ok(consultasPendentes);
        }

        [HttpPost("responder")]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> ResponderConsulta([FromBody] ResponderConsultaRequest request)
        {
            await _consultaService.ResponderConsultaAsync(request);
            return Ok(new { message = request.Aceitar ? "Consulta confirmada com sucesso!" : "Consulta recusada com sucesso!" });
        }
    }
}
