namespace Hackaton.Domain.Requests.Notificacao
{
    public class UpdateNotificacaoRequest 
    {
        public string Id { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataEnvio { get; set; }
    }
}
