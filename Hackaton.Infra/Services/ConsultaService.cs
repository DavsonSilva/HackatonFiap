using AutoMapper;
using Hackaton.Domain.Entities.ConsultaEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Domain.Requests.Consulta;
using Hackaton.Domain.Requests.Notificacao;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;

namespace Hackaton.Infra.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IAgendaRepository _agendaRepository;
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;

        

        public ConsultaService(IConsultaRepository consultaRepository, IAgendaRepository agendaRepository, INotificacaoService notificacaoService, IMapper mapper)
        {
            _consultaRepository = consultaRepository;
            _agendaRepository = agendaRepository;
            _notificacaoService = notificacaoService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ConsultaResponse>> GetAllAsync()
        {
            var consultas = await _consultaRepository.AllAsync();
            return _mapper.Map<IEnumerable<ConsultaResponse>>(consultas);
        }

        public async Task<ConsultaResponse> GetByIdAsync(int id)
        {
            var consulta = await _consultaRepository.FindByIdAsync(id);
            return _mapper.Map<ConsultaResponse>(consulta);
        }

        public async Task AddAsync(CreateConsultaRequest request)
        {
            var agenda = await _agendaRepository.FindByIdAsync(request.AgendaId);
            if (agenda == null || !agenda.Disponivel)
                throw new System.Exception("Horário indisponível.");

            agenda.Disponivel = false;
            await _agendaRepository.UpdateAsync(agenda);

            var consulta = _mapper.Map<Consulta>(request);
            consulta.DataHora = agenda.DataHora;

            await _consultaRepository.AddAsync(consulta);

            var notificacao = new CreateNotificacaoRequest
            {
                UsuarioId = request.MedicoId,
                Mensagem = $"Uma nova consulta foi marcada para {agenda.DataHora}."
            };

            await _notificacaoService.AddAsync(notificacao);
        }
        public async Task CancelarConsultaAsync(CancelConsultaRequest request)
        {
            var consulta = await _consultaRepository.FindByIdAsync(request.ConsultaId);
            if (consulta == null)
                throw new System.Exception("Consulta não encontrada.");

            var agenda = await _agendaRepository.FindByIdAsync(consulta.AgendaId);
            if (agenda != null)
            {
                agenda.Disponivel = true;
                await _agendaRepository.UpdateAsync(agenda);
            }

            await _consultaRepository.DeleteAsync(consulta.Id);

            var notificacao = new CreateNotificacaoRequest
            {
                UsuarioId = consulta.MedicoId,
                Mensagem = $"A consulta marcada para {consulta.DataHora} foi cancelada. Motivo: {request.Motivo}"
            };

            await _notificacaoService.AddAsync(notificacao);
        }

        public async Task UpdateAsync(UpdateConsultaRequest request)
        {
            var consulta = _mapper.Map<Consulta>(request);
            await _consultaRepository.UpdateAsync(consulta);
        }

        public async Task DeleteAsync(int id)
        {
            await _consultaRepository.DeleteAsync(id);
        }

        public async Task<ConsultaDetalhadaResponse> GetConsultaDetalhadaAsync(int id)
        {
            var consulta = await _consultaRepository.GetByIdAsync(id);
            return _mapper.Map<ConsultaDetalhadaResponse>(consulta);
        }

        public async Task<IEnumerable<ConsultaResponse>> GetHistoricoPacienteAsync(int pacienteId)
        {
            var consultas = await _consultaRepository.GetHistoricoPacienteAsync(pacienteId);
            return _mapper.Map<IEnumerable<ConsultaResponse>>(consultas);
        }

        public async Task<IEnumerable<ConsultaResponse>> GetHistoricoMedicoAsync(int medicoId)
        {
            var consultas = await _consultaRepository.GetHistoricoMedicoAsync(medicoId);
            return _mapper.Map<IEnumerable<ConsultaResponse>>(consultas);
        }
    }
}
