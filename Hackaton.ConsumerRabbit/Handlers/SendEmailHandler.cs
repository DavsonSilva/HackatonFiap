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
        await _sendGridService.SendAppointmentNotificationAsync(request.MedicoEmail, request.NomeMedico, request.NomePaciente,
            request.Data, request.Horario);
    }
    
}