using System.ComponentModel.DataAnnotations.Schema;
using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Entities.BaseEntity;

namespace Hackaton.Domain.Entities.AgendaEntity
{
    public class Agenda : Entity
    {

        [ForeignKey("Medico")]
        public int MedicoId { get; set; }
        public virtual Medico Medico { get; set; }

        public DateTime DataHora { get; set; }

        public bool Disponivel { get; set; } = true;
    }
}
