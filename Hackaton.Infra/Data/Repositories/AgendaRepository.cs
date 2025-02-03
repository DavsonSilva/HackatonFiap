using Hackaton.Domain.Entities.AgendaEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Infra.Data.Repositories
{
    public class AgendaRepository : BaseRepository<Agenda>, IAgendaRepository
    {
        public AgendaRepository(FiapDbContext context) : base(context)
        {
        }
        public async Task<Agenda> GetByIdAsync(int id)
        {
            return await set.FindAsync(id);
        }

        public async Task<IEnumerable<Agenda>> GetAllWithDetailsAsync()
        {
            return await set
                .Include(a => a.Medico)  
                .Include(a => a.Paciente) 
                .ToListAsync();
        }

        public async Task<Agenda> GetByIdWithDetailsAsync(int id)
        {
            return await set
                .Include(a => a.Medico)  
                .Include(a => a.Paciente) 
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAgendaAsync(Agenda agenda)
        {
            set.Update(agenda);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAgendaAsync(int id)
        {
            var agenda = await GetByIdAsync(id);
            if (agenda != null)
            {
                set.Remove(agenda);
                await _context.SaveChangesAsync();
            }
        }
    }
}
