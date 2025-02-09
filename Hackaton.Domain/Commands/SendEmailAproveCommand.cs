using MediatR;

namespace Hackaton.Domain.Commands
{
    public class SendEmailAproveCommand : IRequest
    {
        public SendEmailAproveCommand(string medicoEmail, string nomeMedico, string nomePaciente, string data, string horario)
        {
            MedicoEmail = medicoEmail;
            NomeMedico = nomeMedico;
            NomePaciente = nomePaciente;
            Data = data;
            Horario = horario;
        }

        public string MedicoEmail { get; set; }
        public string NomeMedico { get; set; }
        public string NomePaciente { get; set; }
        public string Data { get; set; }
        public string Horario { get; set; }
    }
}