using Hackaton.Domain.Requests.Paciente;
using Hackaton.Domain.Responses;

namespace Hackaton.Domain.Services
{
    public interface IPacienteService
    {
        Task<IEnumerable<PacienteResponse>> GetAllAsync();
        Task<PacienteResponse> GetByIdAsync(int id);
        Task AddAsync(CreatePacienteRequest request);
        Task UpdateAsync(UpdatePacienteRequest request);
        Task DeleteAsync(int id);
    }
}
