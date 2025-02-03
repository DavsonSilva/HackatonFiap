namespace Hackaton.Domain.Responses
{
    public class ConsultaResponse
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string NomePaciente { get; set; } // ✅ Nome do paciente
        public int MedicoId { get; set; }
        public string NomeMedico { get; set; } // ✅ Nome do médico
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
