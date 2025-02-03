using Hackaton.Domain.Requests.Consulta;
using Hackaton.Domain.Responses;

namespace Hackaton.Domain.Services
{
    public interface IConsultaService
    {
        Task<IEnumerable<ConsultaResponse>> GetAllAsync();
        Task<ConsultaResponse> GetByIdAsync(int id);
        Task AddAsync(CreateConsultaRequest request);
        Task<PacienteHistoricoResponse> GetHistoricoPacienteAsync(int pacienteId);
        Task<MedicoHistoricoResponse> GetHistoricoMedicoAsync(int medicoId);
        Task CancelarConsultaAsync(CancelConsultaRequest request);
        Task UpdateAsync(UpdateConsultaRequest request);
        Task DeleteAsync(int id);



        Task<ConsultaResponse> AgendarConsultaAsync(CreateConsultaRequest request);
    }
}
