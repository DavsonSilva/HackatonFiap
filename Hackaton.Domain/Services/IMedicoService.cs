using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Requests.Agenda;
using Hackaton.Domain.Requests.Medico;
using Hackaton.Domain.Responses;
using System.Threading.Tasks;

namespace Hackaton.Domain.Services
{
    public interface IMedicoService
    {
        Task<IEnumerable<MedicoResponse>> GetAllAsync();
        Task<MedicoResponse> GetByIdAsync(int id);
        Task<MedicoResponse> AddAsync(CreateMedicoRequest request);
        Task UpdateAsync(UpdateMedicoRequest request);
        Task DeleteAsync(int id);

        //Agenda
        Task AddAgendaAsync(int medicoId, List<CreateAgendaRequest> agendaRequests);
        Task EditarAgendaAsync(int medicoId, int agendaId, UpdateAgendaRequest request);
        Task ExcluirAgendaAsync(int medicoId, int agendaId);
    }
}
