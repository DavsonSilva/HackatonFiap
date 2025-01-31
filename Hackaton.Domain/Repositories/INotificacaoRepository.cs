using Hackaton.Domain.Entities.NotificacaoEntity;

namespace Hackaton.Domain.Repositories
{
    public interface INotificacaoRepository : IBaseRepository<Notificacao>
    {
        Task<IEnumerable<Notificacao>> GetAllAsync();
        Task<IEnumerable<Notificacao>> GetByUsuarioIdAsync(int usuarioId);
        Task AddAsync(Notificacao notificacao);
    }
}
