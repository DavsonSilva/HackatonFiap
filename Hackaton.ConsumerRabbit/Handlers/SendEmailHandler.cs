using Hackaton.Domain.Commands;
using Hackaton.Domain.Services;
using MediatR;

namespace Hackaton.ConsumerRabbit.Handlers;

public class SendEmailHandler : IRequestHandler<SendEmailCommand>
{
    private readonly ISendGridService _sendGridService;
    
    public SendEmailHandler(ISendGridService sendGridService)
    {
        _sendGridService = sendGridService;
    }
    
    public async Task Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        await _sendGridService.EnviarEmailConsulta(request.MedicoEmail, request.NomeMedico, request.NomePaciente,
            request.Data, request.Horario);
    }
}