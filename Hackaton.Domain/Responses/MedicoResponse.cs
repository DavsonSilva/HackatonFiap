namespace Hackaton.Domain.Responses
{
    public class MedicoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CRM { get; set; }
        public string Email { get; set; }
        public List<AgendaResponse> HorariosDisponiveis { get; set; }
    }
}
