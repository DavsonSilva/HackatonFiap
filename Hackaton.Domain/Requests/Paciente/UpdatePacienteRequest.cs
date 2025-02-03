using Hackaton.Domain.Requests.Consulta;

namespace Hackaton.Domain.Requests.Paciente
{
    public class UpdatePacienteRequest
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string? Senha { get; set; }
    }
}
