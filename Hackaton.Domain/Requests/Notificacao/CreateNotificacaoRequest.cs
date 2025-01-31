namespace Hackaton.Domain.Requests.Notificacao
{
    public class CreateNotificacaoRequest
    {
        public int UsuarioId { get; set; }
        public string Mensagem { get; set; }
    }
}
