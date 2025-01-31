using Hackaton.Domain.Entities.PacienteEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Infra.Data.Context;

namespace Hackaton.Infra.Data.Repositories
{
    public class PacienteRepository : BaseRepository<Paciente>, IPacienteRepository
    {
        public PacienteRepository(FiapDbContext context) : base(context)
        {
        }
    }
}