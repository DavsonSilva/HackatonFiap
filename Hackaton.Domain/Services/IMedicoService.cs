using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Requests.Medico;
using Hackaton.Domain.Responses;
using System.Threading.Tasks;

namespace Hackaton.Domain.Services
{
    public interface IMedicoService
    {
        Task<IEnumerable<MedicoResponse>> GetAllAsync();
        Task<MedicoResponse> GetByIdAsync(int id);
        Task<IEnumerable<MedicoResponse>> SearchAsync(string query);
        Task AddAsync(CreateMedicoRequest request);
        Task UpdateAsync(UpdateMedicoRequest request);
        Task DeleteAsync(int id);
    }
}
