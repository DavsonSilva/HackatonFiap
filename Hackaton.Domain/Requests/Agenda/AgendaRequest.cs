namespace Hackaton.Domain.Requests.Agenda
{
    public class AgendaRequest 
    {
        public string Id { get; set; }
        public DateTime DataHora { get; set; }
        public bool Disponivel { get; set; }
    }
}
