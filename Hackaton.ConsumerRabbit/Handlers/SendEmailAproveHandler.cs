using Hackaton.Domain.Commands;
using Hackaton.Domain.Services;
using MediatR;

namespace Hackaton.ConsumerRabbit.Handlers
{
    public class SendEmailAproveHandler : IRequestHandler<SendEmailAproveCommand>
    {
        private readonly ISendGridService _sendGridService;

        public SendEmailAproveHandler(ISendGridService sendGridService)
        {
            _sendGridService = sendGridService;
        }

        public async Task Handle(SendEmailAproveCommand request, CancellationToken cancellationToken)
        {
            await _sendGridService.EnviarEmailAprovarConsulta(request.MedicoEmail, request.NomeMedico, request.NomePaciente,
                request.Data, request.Horario);
        }
    }
}