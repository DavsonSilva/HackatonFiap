using Hackaton.Domain.Requests.Base;

namespace Hackaton.Domain.Requests.Medico
{
    public class ListMedicoRequest : BaseConsultaPaginada
    {
        public string? Query { get; set; }
    }
}
