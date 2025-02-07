using Hackaton.Domain.Requests.Base;
using Hackaton.Domain.Requests.Consulta;
using Hackaton.Domain.Responses;

namespace Hackaton.Domain.Services
{
    public interface IConsultaService
    {
        Task<ConsultaListaResponse> GetAllAsync(BaseConsultaPaginada request);
        Task<PacienteHistoricoResponse> GetHistoricoPacienteAsync(int pacienteId);
        Task<MedicoHistoricoResponse> GetHistoricoMedicoAsync(int medicoId);
        Task CancelarConsultaAsync(CancelConsultaRequest request);
        Task<ConsultaResponse> AgendarConsultaAsync(CreateConsultaRequest request);
        Task ResponderConsultaAsync(ResponderConsultaRequest request);
        Task<IEnumerable<ConsultaResponse>> GetPendentesPorMedicoAsync(int medicoId);
        Task<IEnumerable<ConsultaResponse>> GetPendentesAsync(int medicoId);
    }
}
