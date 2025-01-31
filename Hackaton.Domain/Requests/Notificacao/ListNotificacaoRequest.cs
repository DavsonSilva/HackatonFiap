using Hackaton.Domain.Requests.Base;

namespace Hackaton.Domain.Requests.Notificacao
{
    public class ListNotificacaoRequest : BaseConsultaPaginada
    {
        public string? Query { get; set; }
    }
}