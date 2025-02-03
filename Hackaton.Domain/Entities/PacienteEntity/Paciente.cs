using Hackaton.Domain.Entities.ConsultaEntity;
using Hackaton.Domain.Entities.UsuarioEntity;
using System.ComponentModel.DataAnnotations;

namespace Hackaton.Domain.Entities.PacienteEntity
{
    public class Paciente : Usuario
    {
        [Required]
        [MaxLength(14)] 
        public string CPF { get; set; }
        public virtual List<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}
