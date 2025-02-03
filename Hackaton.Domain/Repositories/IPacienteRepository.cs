using Hackaton.Domain.Entities.PacienteEntity;

namespace Hackaton.Domain.Repositories
{
    public interface IPacienteRepository : IBaseRepository<Paciente>
    {
        Task<Paciente> FindByEmailOrCPFAsync(string email, string cpf);
    }
}
