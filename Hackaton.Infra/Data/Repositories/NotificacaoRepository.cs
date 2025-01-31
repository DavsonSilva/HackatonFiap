using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Entities.NotificacaoEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Infra.Data.Repositories
{
    public class NotificacaoRepository : BaseRepository<Notificacao>, INotificacaoRepository
    {
        public NotificacaoRepository(FiapDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notificacao>> GetAllAsync()
        {
            return await set.ToListAsync();
        }

        public async Task<IEnumerable<Notificacao>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await set.Where(n => n.UsuarioId == usuarioId).ToListAsync();
        }

        public async Task AddAsync(Notificacao notificacao)
        {
            await set.AddAsync(notificacao);
            await _context.SaveChangesAsync();
        }
    }
}