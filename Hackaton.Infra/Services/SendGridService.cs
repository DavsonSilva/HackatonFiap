using Hackaton.Domain.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Hackaton.Infra.Services
{
    public class SendGridService : ISendGridService
    {
        private readonly IConfiguration _configuration;
        private readonly ISendGridClient _sendGridClient;

        public SendGridService(
            IConfiguration configuration,
            ISendGridClient sendGridClient)
        {
            _configuration = configuration;
            _sendGridClient = sendGridClient;
        }

        public async Task<string> EnviarEmailConsulta(string medicoEmail, string nomeMedico, string nomePaciente, string data, string horario)
        {
            try
            {
                string fromEmail = _configuration["SendGridEmailSettings:FromEmail"];
                string fromName = _configuration["SendGridEmailSettings:FromName"];

                var subject = "Health&Med - Nova consulta agendada";

                var plainTextContent = $"Olá, Dr. {nomeMedico}!\n\n" +
                                       $"Você tem uma nova consulta marcada!\n" +
                                       $"Paciente: {nomePaciente}.\n" +
                                       $"Data e horário: {data} às {horario}.";

                var htmlContent = $@"<!DOCTYPE html>
                                    <html>
                                    <body>
                                    <p>Olá, Dr. {nomeMedico}!</p>
                                    <p>Você tem uma nova consulta marcada!</p>
                                    <p><strong>Paciente:</strong> {nomePaciente}</p>
                                    <p><strong>Data e horário:</strong> {data} às {horario}</p>
                                    </body>
                                    </html>";

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(fromEmail, fromName),
                    Subject = subject,
                    PlainTextContent = plainTextContent,
                    HtmlContent = htmlContent
                };

                msg.AddTo("davson.silva@auditeste.com.br");
                var response = await _sendGridClient.SendEmailAsync(msg);

                return response.IsSuccessStatusCode ? "E-mail de notificação enviado com sucesso" : $"Falha ao enviar a notificação. Status Code: {response.StatusCode}";
            }
            catch (Exception ex)
            {
                return $"Erro ao enviar a notificação: {ex.Message}";
            }
        }

        public async Task<string> EnviarEmailAprovarConsulta(string medicoEmail, string nomeMedico, string nomePaciente, string data, string horario)
        {
            try
            {
                string fromEmail = _configuration["SendGridEmailSettings:FromEmail"];
                string fromName = _configuration["SendGridEmailSettings:FromName"];

                var subject = $"Health&Med - Aprovar a consulta";

                var plainTextContent = $"Olá, Dr. {nomeMedico}!\n\n" +
                                       $"Você tem uma nova consulta para aprovar!\n" +
                                       $"Paciente: {nomePaciente}.\n" +
                                       $"Data e horário: {data} às {horario}.";

                var htmlContent = $@"<!DOCTYPE html>
                                    <html>
                                    <body>
                                    <p>Olá, Dr. {nomeMedico}!</p>
                                    <p>Você tem uma nova consulta para aprovar!</p>
                                    <p><strong>Paciente:</strong> {nomePaciente}</p>
                                    <p><strong>Data e horário:</strong> {data} às {horario}</p>
                                    </body>
                                    </html>";

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(fromEmail, fromName),
                    Subject = subject,
                    PlainTextContent = plainTextContent,
                    HtmlContent = htmlContent
                };

                msg.AddTo("davson.silva@auditeste.com.br");
                var response = await _sendGridClient.SendEmailAsync(msg);

                return response.IsSuccessStatusCode ? "E-mail de notificação enviado com sucesso" : $"Falha ao enviar a notificação. Status Code: {response.StatusCode}";
            }
            catch (Exception ex)
            {
                return $"Erro ao enviar a notificação: {ex.Message}";
            }
        }

        public async Task<string> EnviarEmailConsultaCancelada(string medicoEmail, string nomeMedico, string nomePaciente, string data, string horario)
        {
            try
            {
                string fromEmail = _configuration["SendGridEmailSettings:FromEmail"];
                string fromName = _configuration["SendGridEmailSettings:FromName"];

                var subject = "Health&Med - Cancelamento de consulta";

                var plainTextContent = $"Olá, Dr. {nomeMedico}!\n\n" +
                                       $"Você tem uma nova consulta Cancelada!\n" +
                                       $"Paciente: {nomePaciente}.\n" +
                                       $"Data e horário: {data} às {horario}.";

                var htmlContent = $@"<!DOCTYPE html>
                                    <html>
                                    <body>
                                    <p>Olá, Dr. {nomeMedico}!</p>
                                    <p>Você tem uma nova consulta cancelada!</p>
                                    <p><strong>Paciente:</strong> {nomePaciente}</p>
                                    <p><strong>Data e horário:</strong> {data}</p>
                                    <p><strong>Motivo do acancelamento:</strong> {horario} às</p>
                                    </body>
                                    </html>";

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(fromEmail, fromName),
                    Subject = subject,
                    PlainTextContent = plainTextContent,
                    HtmlContent = htmlContent
                };

                msg.AddTo("davson.silva@auditeste.com.br");
                var response = await _sendGridClient.SendEmailAsync(msg);

                return response.IsSuccessStatusCode ? "E-mail de notificação enviado com sucesso" : $"Falha ao enviar a notificação. Status Code: {response.StatusCode}";
            }
            catch (Exception ex)
            {
                return $"Erro ao enviar a notificação: {ex.Message}";
            }
        }
    }
}
