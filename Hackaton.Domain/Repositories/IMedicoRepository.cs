using Hackaton.Domain.Entities.MedicoEntity;

namespace Hackaton.Domain.Repositories
{
    public interface IMedicoRepository : IBaseRepository<Medico>
    {
        Task<IEnumerable<Medico>> GetAllAsync();
        Task<Medico> GetByIdAsync(int id);
        Task<IEnumerable<Medico>> SearchAsync(string query);
        Task AddAsync(Medico medico);
        Task UpdateAsync(Medico medico);
        Task DeleteAsync(int id);
    }
}
