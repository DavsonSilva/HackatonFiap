using Hackaton.Domain.Requests.Notificacao;
using Hackaton.Domain.Responses;

namespace Hackaton.Domain.Services
{
    public interface INotificacaoService
    {
        Task<IEnumerable<NotificacaoResponse>> GetAllAsync();
        Task<NotificacaoResponse> GetByIdAsync(int id);
        Task AddAsync(CreateNotificacaoRequest request);
        Task<IEnumerable<NotificacaoResponse>> GetByUsuarioIdAsync(int usuarioId);
        Task DeleteAsync(int id);
        Task EnviarNotificacaoAsync(int usuarioId, string mensagem);
    }
}
