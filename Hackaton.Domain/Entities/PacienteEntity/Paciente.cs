using Hackaton.Domain.Entities.ConsultaEntity;
using Hackaton.Domain.Entities.UsuarioEntity;

namespace Hackaton.Domain.Entities.PacienteEntity
{
    public class Paciente : Usuario
    {
        public virtual List<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}
