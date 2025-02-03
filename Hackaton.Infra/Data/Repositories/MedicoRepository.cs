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
            return await set
                .Include(m => m.HorariosDisponiveis)
                    .ThenInclude(a => a.Paciente)
                .ToListAsync();
        }

        public async Task<Medico> GetByIdAsync(int id)
        {
            return await set
                .Include(m => m.HorariosDisponiveis)
                    .ThenInclude(a => a.Paciente) 
                .FirstOrDefaultAsync(m => m.Id == id);
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