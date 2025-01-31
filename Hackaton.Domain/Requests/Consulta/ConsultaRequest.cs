namespace Hackaton.Domain.Requests.Consulta
{
    public class ConsultaRequest 
    {
        public string Id { get; set; }
        public string NomeMedico { get; set; }
        public string NomePaciente { get; set; }
        public DateTime DataHora { get; set; }
        public bool Confirmada { get; set; }
    }
}
