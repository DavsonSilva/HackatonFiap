using Hackaton.Domain.Entities.PacienteEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Infra.Data.Repositories
{
    public class PacienteRepository : BaseRepository<Paciente>, IPacienteRepository
    {
        public PacienteRepository(FiapDbContext context) : base(context)
        {
        }
        public async Task<Paciente> FindByEmailOrCPFAsync(string email, string cpf)
        {
            return await set.FirstOrDefaultAsync(p => p.Email == email || p.CPF == cpf);
        }
    }
}