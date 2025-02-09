using Hackaton.Domain.Commands;
using Hackaton.Domain.Services;
using MediatR;

namespace Hackaton.ConsumerRabbit.Handlers
{
    public class SendEmailCancelHandler : IRequestHandler<SendEmailCancelCommand>
    {
        private readonly ISendGridService _sendGridService;

        public SendEmailCancelHandler(ISendGridService sendGridService)
        {
            _sendGridService = sendGridService;
        }

        public async Task Handle(SendEmailCancelCommand request, CancellationToken cancellationToken)
        {
            await _sendGridService.EnviarEmailConsultaCancelada(request.MedicoEmail, request.NomeMedico, request.NomePaciente, 
                request.Data, request.Horario);
        }
    }
}