using Hackaton.Domain.Requests.Agenda;
using Hackaton.Domain.Responses;

namespace Hackaton.Domain.Services
{
    public interface IAgendaService
    {
        Task<IEnumerable<AgendaResponse>> GetAllAsync();
        Task<AgendaResponse> GetByIdAsync(int id);
    }
}
