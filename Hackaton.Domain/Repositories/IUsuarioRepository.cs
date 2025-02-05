using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Entities.UsuarioEntity;

namespace Hackaton.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetByEmailAndPasswordAsync(string email);
        Task<Usuario> GetByIdAsync(int id);
        Task<Medico> GetMedicoByCrmAndPasswordAsync(string crm);
    }
}
