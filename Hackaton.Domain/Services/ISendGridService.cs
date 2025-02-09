namespace Hackaton.Domain.Services
{
    public interface ISendGridService
    {
        Task<string> EnviarEmailConsulta(string medicoEmail, string nomeMedico, string nomePaciente, string data, string horario);
        Task<string> EnviarEmailAprovarConsulta(string medicoEmail, string nomeMedico, string nomePaciente, string data, string horario);
        Task<string> EnviarEmailConsultaCancelada(string medicoEmail, string nomeMedico, string nomePaciente , string data, string horario);
    }
}
