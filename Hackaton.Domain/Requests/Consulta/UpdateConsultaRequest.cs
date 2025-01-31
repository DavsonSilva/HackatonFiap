namespace Hackaton.Domain.Requests.Consulta
{
    public class UpdateConsultaRequest
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public int AgendaId { get; set; }
    }
}
