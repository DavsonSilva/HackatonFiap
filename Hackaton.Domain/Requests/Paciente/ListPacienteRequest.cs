using Hackaton.Domain.Requests.Base;

namespace Hackaton.Domain.Requests.Paciente
{
    public class ListPacienteRequest : BaseConsultaPaginada
    {
        public string? Query { get; set; }
    }
}
