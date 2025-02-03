using Hackaton.Domain.Entities.UsuarioEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Infra.Data.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        private readonly FiapDbContext _context; 

        public UsuarioRepository(FiapDbContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<Usuario> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await set.FirstOrDefaultAsync(u => u.Email == email && u.Senha == password);
        }

        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuario.FindAsync(id);
        }
    }
}
