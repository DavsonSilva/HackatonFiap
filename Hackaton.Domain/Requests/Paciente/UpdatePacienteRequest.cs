using Hackaton.Domain.Requests.Consulta;

namespace Hackaton.Domain.Requests.Paciente
{
    public class UpdatePacienteRequest
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
    }
}
