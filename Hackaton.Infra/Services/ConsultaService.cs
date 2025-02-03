using AutoMapper;
using Hackaton.Domain.Entities.ConsultaEntity;
using Hackaton.Domain.Entities.PacienteEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Domain.Requests.Consulta;
using Hackaton.Domain.Requests.Notificacao;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;
using Hackaton.Infra.Data.Repositories;

namespace Hackaton.Infra.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IAgendaRepository _agendaRepository;
        private readonly IMedicoRepository _medicoRepository;
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _usuarioRepository;



        public ConsultaService(IConsultaRepository consultaRepository, IAgendaRepository agendaRepository, INotificacaoService notificacaoService, IMapper mapper, IMedicoRepository medicoRepository, IUsuarioRepository usuarioRepository)
        {
            _consultaRepository = consultaRepository;
            _agendaRepository = agendaRepository;
            _notificacaoService = notificacaoService;
            _mapper = mapper;
            _medicoRepository = medicoRepository;
            _usuarioRepository = usuarioRepository;
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
            var agenda = await _agendaRepository.FindByIdAsync(request.PacienteId);
            if (agenda == null || !agenda.Disponivel)
                throw new System.Exception("Horário indisponível.");

            agenda.Disponivel = false;
            await _agendaRepository.UpdateAsync(agenda);

            var consulta = _mapper.Map<Consulta>(request);
            consulta.DataHora = agenda.DataHora;

            await _consultaRepository.AddAsync(consulta);

            var notificacao = new CreateNotificacaoRequest
            {
                UsuarioId = request.PacienteId,
                Mensagem = $"Uma nova consulta foi marcada para {agenda.DataHora}."
            };

            await _notificacaoService.AddAsync(notificacao);
        }
        public async Task CancelarConsultaAsync(CancelConsultaRequest request)
        {
            // 1️⃣ Buscar a consulta no banco
            var consulta = await _consultaRepository.GetByIdAsync(request.ConsultaId);
            if (consulta == null)
            {
                throw new Exception("Consulta não encontrada.");
            }

            // 2️⃣ Buscar a agenda associada à consulta
            var agenda = await _agendaRepository.GetByIdAsync(consulta.AgendaId);
            if (agenda == null)
            {
                throw new Exception("Erro ao recuperar a agenda da consulta.");
            }

            // 3️⃣ Liberar o horário na agenda
            agenda.Disponivel = true;
            agenda.PacienteId = null; // Limpa o paciente agendado
            await _agendaRepository.UpdateAsync(agenda);

            // 4️⃣ Remover a consulta do banco
            await _consultaRepository.DeleteAsync(consulta.Id);

            // 5️⃣ Criar notificação para o médico
            var notificacao = new CreateNotificacaoRequest
            {
                UsuarioId = consulta.MedicoId, // Notifica o médico
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

        public async Task<ConsultaResponse> AgendarConsultaAsync(CreateConsultaRequest request)
        {
            // 1️⃣ Buscar o horário na Agenda
            var agenda = await _agendaRepository.GetByIdAsync(request.AgendaId);
            if (agenda == null || !agenda.Disponivel)
            {
                throw new Exception("O horário selecionado não está disponível.");
            }

            // 2️⃣ Buscar o paciente
            var paciente = await _usuarioRepository.GetByIdAsync(request.PacienteId);
            if (paciente == null)
            {
                throw new Exception("Paciente inválido.");
            }

            // 3️⃣ Criar a consulta
            var consulta = new Consulta
            {
                PacienteId = request.PacienteId,
                MedicoId = agenda.MedicoId, // ✅ Obtém MedicoId diretamente da Agenda
                AgendaId = request.AgendaId,
                DataHora = agenda.DataHora
            };

            // 4️⃣ Atualizar a Agenda (associando paciente e removendo a disponibilidade)
            agenda.PacienteId = request.PacienteId;
            agenda.Disponivel = false;

            // 5️⃣ Salvar no banco
            await _consultaRepository.AddAsync(consulta);
            await _agendaRepository.UpdateAsync(agenda);

            // 6️⃣ Retornar resposta
            return new ConsultaResponse
            {
                Id = consulta.Id,
                PacienteId = consulta.PacienteId,
                MedicoId = consulta.MedicoId, // ✅ Agora a resposta terá MedicoId
                AgendaId = consulta.AgendaId,
                DataHora = consulta.DataHora
            };
        }
    }
}
