using System.ComponentModel.DataAnnotations.Schema;
using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Entities.PacienteEntity;
using Hackaton.Domain.Entities.BaseEntity;
using Hackaton.Domain.Entities.AgendaEntity;
using Hackaton.Domain.Enum;

namespace Hackaton.Domain.Entities.ConsultaEntity
{
    public class Consulta : Entity
    {

        [ForeignKey("Medico")]
        public int MedicoId { get; set; }
        public virtual Medico Medico { get; set; }

        [ForeignKey("Paciente")]
        public int PacienteId { get; set; }
        public virtual Paciente Paciente { get; set; }

        [ForeignKey("Agenda")]
        public int AgendaId { get; set; }
        public virtual Agenda Agenda { get; set; }

        public DateTime DataHora { get; set; }
        public StatusConsulta Status { get; set; } = StatusConsulta.Pendente;
    }
}
