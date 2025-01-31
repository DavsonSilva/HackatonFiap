using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Infra.Data.Repositories
{
    public class MedicoRepository : BaseRepository<Medico>, IMedicoRepository
    {
        public MedicoRepository(FiapDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Medico>> GetAllAsync()
        {
            return await set.ToListAsync();
        }

        public async Task<Medico> GetByIdAsync(int id)
        {
            return await set.FindAsync(id);
        }

        public async Task<IEnumerable<Medico>> SearchAsync(string query)
        {
            return await set
                .Where(m => m.Nome.Contains(query) || m.CRM.Contains(query))
                .ToListAsync();
        }

        public async Task AddAsync(Medico medico)
        {
            await set.AddAsync(medico);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Medico medico)
        {
            set.Update(medico);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var medico = await GetByIdAsync(id);
            if (medico != null)
            {
                set.Remove(medico);
                await _context.SaveChangesAsync();
            }
        }
    }
}