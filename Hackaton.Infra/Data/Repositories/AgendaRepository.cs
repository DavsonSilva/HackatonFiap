using Hackaton.Domain.Entities.AgendaEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Infra.Data.Context;

namespace Hackaton.Infra.Data.Repositories
{
    public class AgendaRepository : BaseRepository<Agenda>, IAgendaRepository
    {
        public AgendaRepository(FiapDbContext context) : base(context)
        {
        }

    }
}
