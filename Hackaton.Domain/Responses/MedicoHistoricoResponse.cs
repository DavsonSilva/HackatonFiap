using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Domain.Responses
{
    public class MedicoHistoricoResponse
    {
        public int MedicoId { get; set; }
        public string NomeMedico { get; set; }
        public List<ConsultaDetalhadaMedicoResponse> Consultas { get; set; } = new List<ConsultaDetalhadaMedicoResponse>();
    }

    public class ConsultaDetalhadaMedicoResponse
    {
        public int Id { get; set; }
        public string NomePaciente { get; set; }
        public DateTime DataHora { get; set; }
    }
}
