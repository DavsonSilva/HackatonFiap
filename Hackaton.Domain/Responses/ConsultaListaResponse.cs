namespace Hackaton.Domain.Responses
{
    public class ConsultaListaResponse
    {
        public List<MedicoComConsultasResponse> Medicos { get; set; } = new();
        public int TotalConsultas { get; set; } 
        public int PaginaAtual { get; set; } 
        public int TotalPaginas { get; set; }
    }

    public class MedicoComConsultasResponse
    {
        public int MedicoId { get; set; }
        public string NomeMedico { get; set; }
        public List<ConsultaDetalhadaResponse> Consultas { get; set; } = new();
    }

    public class ConsultaDetalhadaResponse
    {
        public int Id { get; set; }
        public string NomePaciente { get; set; }
        public DateTime DataHora { get; set; }
    }
}
