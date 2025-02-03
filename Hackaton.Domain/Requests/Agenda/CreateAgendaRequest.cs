namespace Hackaton.Domain.Requests.Agenda
{
    public class CreateAgendaRequest
    {
        public DateTime DataHora { get; set; }
        public bool Disponivel { get; set; }
    }
}
