using Hackaton.Domain.Requests.Agenda;

namespace Hackaton.Domain.Requests.Medico
{
    public class UpdateMedicoRequest 
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CRM { get; set; }
        public string Email { get; set; }
        public string? Senha { get; set; }
    }
}
