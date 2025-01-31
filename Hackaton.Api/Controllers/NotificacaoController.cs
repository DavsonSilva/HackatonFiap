using Hackaton.Domain.Requests.Notificacao;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificacaoController : ControllerBase
    {
        private readonly INotificacaoService _notificacaoService;

        public NotificacaoController(INotificacaoService notificacaoService)
        {
            _notificacaoService = notificacaoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificacaoResponse>>> GetAll()
        {
            var notificacoes = await _notificacaoService.GetAllAsync();
            return Ok(notificacoes);
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<NotificacaoResponse>>> GetByUsuarioId(int usuarioId)
        {
            var notificacoes = await _notificacaoService.GetByUsuarioIdAsync(usuarioId);
            return Ok(notificacoes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NotificacaoResponse>> GetById(int id)
        {
            var notificacao = await _notificacaoService.GetByIdAsync(id);
            if (notificacao == null)
                return NotFound();

            return Ok(notificacao);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateNotificacaoRequest request)
        {
            await _notificacaoService.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = request.UsuarioId }, request);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _notificacaoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
