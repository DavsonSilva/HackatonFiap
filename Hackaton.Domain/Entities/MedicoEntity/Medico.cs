using Hackaton.Domain.Entities.AgendaEntity;
using Hackaton.Domain.Entities.UsuarioEntity;
using System.ComponentModel.DataAnnotations;

namespace Hackaton.Domain.Entities.MedicoEntity
{
    public class Medico : Usuario
    {
        [Required]
        [MaxLength(20)]
        public string CRM { get; set; }

        public virtual List<Agenda> HorariosDisponiveis { get; set; } = new List<Agenda>();
    }
}
