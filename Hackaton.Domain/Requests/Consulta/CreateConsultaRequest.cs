namespace Hackaton.Domain.Requests.Consulta
{
    public class CreateConsultaRequest
    {
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public int AgendaId { get; set; }
    }
}
