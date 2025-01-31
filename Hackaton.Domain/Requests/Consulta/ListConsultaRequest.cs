using Hackaton.Domain.Requests.Base;

namespace Hackaton.Domain.Requests.Consulta
{
    public class ListConsultaRequest : BaseConsultaPaginada
    {
        public string? Query { get; set; }
    }
}
