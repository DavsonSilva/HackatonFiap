namespace Hackaton.Domain.Responses
{
    public class ConsultaResponse
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public int AgendaId { get; set; }
        public DateTime DataHora { get; set; }
    }

    public class ConsultaDetalhadaResponse
    {
        public int Id { get; set; }
        public string NomeMedico { get; set; }
        public string NomePaciente { get; set; }
        public DateTime DataHora { get; set; }
    }
}
