namespace Hackaton.Domain.Requests.Agenda
{
    public class CreateAgendaRequest
    {
        public int MedicoId { get; set; }
        public DateTime DataHora { get; set; }
        public bool Disponivel { get; set; }
    }
}
