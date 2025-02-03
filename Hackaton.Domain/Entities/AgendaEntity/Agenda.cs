using System.ComponentModel.DataAnnotations.Schema;
using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Entities.BaseEntity;
using Hackaton.Domain.Entities.PacienteEntity;
using System.ComponentModel.DataAnnotations;

namespace Hackaton.Domain.Entities.AgendaEntity
{
    public class Agenda : Entity
    {
        public int MedicoId { get; set; }
        public Medico Medico { get; set; }

        public int? PacienteId { get; set; } 
        public Paciente? Paciente { get; set; }

        public DateTime DataHora { get; set; }
        public bool Disponivel { get; set; } = true;
    }
}
