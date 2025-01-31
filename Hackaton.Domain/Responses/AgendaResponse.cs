namespace Hackaton.Domain.Responses
{
    public class AgendaResponse
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public DateTime DataHora { get; set; }
        public bool Disponivel { get; set; }
    }
}
