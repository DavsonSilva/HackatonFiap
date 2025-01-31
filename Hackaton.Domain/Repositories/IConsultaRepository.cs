using Hackaton.Domain.Entities.ConsultaEntity;

namespace Hackaton.Domain.Repositories
{
    public interface IConsultaRepository : IBaseRepository<Consulta>
    {
        Task<IEnumerable<Consulta>> GetAllAsync();
        Task<Consulta> GetByIdAsync(int id);
        Task AddAsync(Consulta consulta);
        Task DeleteAsync(int id);
        Task<IEnumerable<Consulta>> GetHistoricoPacienteAsync(int pacienteId);
        Task<IEnumerable<Consulta>> GetHistoricoMedicoAsync(int medicoId);
    }
}
