namespace Hackaton.Domain.Services
{
    public interface ISendGridService
    {
        Task<string> SendAppointmentNotificationAsync(string medicoEmail, string nomeMedico, string nomePaciente, string data, string horario);
    }
}
