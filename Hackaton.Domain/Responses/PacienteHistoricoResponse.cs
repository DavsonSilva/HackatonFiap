namespace Hackaton.Domain.Responses
{
    public class PacienteHistoricoResponse
    {
        public int PacienteId { get; set; }
        public string NomePaciente { get; set; }
        public List<ConsultaDetalhadaPascienteResponse> Consultas { get; set; } = new List<ConsultaDetalhadaPascienteResponse>();
    }

    public class ConsultaDetalhadaPascienteResponse
    {
        public int Id { get; set; }
        public string NomeMedico { get; set; }
        public DateTime DataHora { get; set; }
    }
}
