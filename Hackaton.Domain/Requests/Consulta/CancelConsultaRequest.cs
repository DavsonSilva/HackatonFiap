namespace Hackaton.Domain.Requests.Consulta
{
    public class CancelConsultaRequest
    {
        public int ConsultaId { get; set; }
        public int UsuarioId { get; set; } 
        public string Motivo { get; set; }
    }
}
