using Hackaton.Domain.Entities.ConsultaEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Infra.Data.Repositories
{
    public class ConsultaRepository : BaseRepository<Consulta>, IConsultaRepository
    {
        public ConsultaRepository(FiapDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Consulta>> GetAllAsync()
        {
            return await set
                .Include(c => c.Medico) 
                .Include(c => c.Paciente)
                .OrderByDescending(c => c.DataHora)
                .ToListAsync();
        }

        public IQueryable<Consulta> GetQueryable()
        {
            return set
                .Include(c => c.Medico)
                .Include(c => c.Paciente);
        }

        public async Task<Consulta> GetByIdAsync(int id)
        {
            return await set.FindAsync(id);
        }

        public async Task AddAsync(Consulta consulta)
        {
            await set.AddAsync(consulta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var consulta = await GetByIdAsync(id);
            if (consulta != null)
            {
                set.Remove(consulta);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Consulta>> GetHistoricoPacienteAsync(int pacienteId)
        {
            return await set
                .Where(c => c.PacienteId == pacienteId)
                .Include(c => c.Paciente) 
                .Include(c => c.Medico) 
                .OrderByDescending(c => c.DataHora)
                .ToListAsync();
        }

        public async Task<IEnumerable<Consulta>> GetHistoricoMedicoAsync(int medicoId)
        {
            return await set
                .Where(c => c.MedicoId == medicoId)
                .Include(c => c.Medico) 
                .Include(c => c.Paciente) 
                .OrderByDescending(c => c.DataHora)
                .ToListAsync();
        }
    }
}