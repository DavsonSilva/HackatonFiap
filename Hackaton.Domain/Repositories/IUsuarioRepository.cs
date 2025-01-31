using Hackaton.Domain.Entities.UsuarioEntity;

namespace Hackaton.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetByEmailAndPasswordAsync(string email, string password);
    }
}
