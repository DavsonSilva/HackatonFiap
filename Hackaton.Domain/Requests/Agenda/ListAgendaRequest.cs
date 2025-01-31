using Hackaton.Domain.Requests.Base;

namespace Hackaton.Domain.Requests.Agenda
{
    public class ListAgendaRequest : BaseConsultaPaginada
    {
        public string? Query { get; set; }
    }
}
