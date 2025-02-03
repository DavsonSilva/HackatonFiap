using Hackaton.Domain.Entities.AgendaEntity;

namespace Hackaton.Domain.Repositories
{
    public interface IAgendaRepository : IBaseRepository<Agenda>
    {
        Task<Agenda> GetByIdAsync(int id);
        Task<Agenda> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Agenda>> GetAllWithDetailsAsync();
        Task DeleteAgendaAsync(int id);
    }
}
