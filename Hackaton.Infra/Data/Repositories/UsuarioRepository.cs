using Hackaton.Domain.Entities.MedicoEntity;
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

        public async Task<Usuario> GetByEmailAndPasswordAsync(string email)
        {
            return await set.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Medico> GetMedicoByCrmAndPasswordAsync(string crm)
        {
            return await _context.Set<Medico>().FirstOrDefaultAsync(m => m.CRM == crm);
        }
        public async Task<Usuario> GetByIdAsync(int id)
        {
            return await _context.Usuario.FindAsync(id);
        }
    }
}
