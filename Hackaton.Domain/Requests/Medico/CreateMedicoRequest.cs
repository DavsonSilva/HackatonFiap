using Hackaton.Domain.Requests.Agenda;
namespace Hackaton.Domain.Requests.Medico
{
    public class CreateMedicoRequest
    {
        public string Nome { get; set; }
        public string CRM { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
