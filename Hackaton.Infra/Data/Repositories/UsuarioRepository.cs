using Hackaton.Domain.Entities.UsuarioEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Infra.Data.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(FiapDbContext context) : base(context)
        {
        }

        public async Task<Usuario> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await set.FirstOrDefaultAsync(u => u.Email == email && u.Senha == password);
        }
    }
}
