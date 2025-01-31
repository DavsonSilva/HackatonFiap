using Hackaton.Domain.Requests.Consulta;
using Hackaton.Domain.Responses;

namespace Hackaton.Domain.Services
{
    public interface IConsultaService
    {
        Task<IEnumerable<ConsultaResponse>> GetAllAsync();
        Task<ConsultaResponse> GetByIdAsync(int id);
        Task AddAsync(CreateConsultaRequest request);
        Task<IEnumerable<ConsultaResponse>> GetHistoricoPacienteAsync(int pacienteId);
        Task<IEnumerable<ConsultaResponse>> GetHistoricoMedicoAsync(int medicoId);
        Task CancelarConsultaAsync(CancelConsultaRequest request);
        Task UpdateAsync(UpdateConsultaRequest request);
        Task DeleteAsync(int id);
    }
}
