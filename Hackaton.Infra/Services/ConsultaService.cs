using AutoMapper;
using Hackaton.Domain.Commands;
using Hackaton.Domain.Entities.ConsultaEntity;
using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Entities.PacienteEntity;
using Hackaton.Domain.Enum;
using Hackaton.Domain.Repositories;
using Hackaton.Domain.Requests.Base;
using Hackaton.Domain.Requests.Consulta;
using Hackaton.Domain.Requests.Notificacao;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;
using Hackaton.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hackaton.Infra.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IAgendaRepository _agendaRepository;
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ServicePublisherRabbit _servicePublisherRabbit;
        private readonly IMedicoRepository _medicoRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ISendGridService _sendGridService;

        public ConsultaService(IConsultaRepository consultaRepository, IAgendaRepository agendaRepository,
            INotificacaoService notificacaoService, IMapper mapper, IUsuarioRepository usuarioRepository,
            ServicePublisherRabbit servicePublisherRabbit, IMedicoRepository medicoRepository, IPacienteRepository pacienteRepository, ISendGridService sendGridService)
        {
            _consultaRepository = consultaRepository;
            _agendaRepository = agendaRepository;
            _notificacaoService = notificacaoService;
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
            _servicePublisherRabbit = servicePublisherRabbit;
            _medicoRepository = medicoRepository;
            _pacienteRepository = pacienteRepository;
            _sendGridService = sendGridService;
        }

        public async Task<ConsultaListaResponse> GetAllAsync(BaseConsultaPaginada request)
        {
            var consultasQuery = _consultaRepository.GetQueryable()
                .Include(c => c.Medico)
                .Include(c => c.Paciente)
                .OrderByDescending(c => c.DataHora)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                string searchTerm = request.Search.ToLower();

                consultasQuery = consultasQuery.Where(c =>
                    EF.Functions.ILike(c.Medico.Nome, $"%{searchTerm}%") ||
                    EF.Functions.ILike(c.Paciente.Nome, $"%{searchTerm}%"));
            }

            var totalConsultas = await consultasQuery.CountAsync();
            var consultas = await consultasQuery
                .Skip((request.Page - 1) * request.Count)
                .Take(request.Count)
                .ToListAsync();

            if (!consultas.Any())
            {
                return new ConsultaListaResponse();
            }

            var medicos = consultas
                .GroupBy(c => c.MedicoId)
                .Select(grupo => new MedicoComConsultasResponse
                {
                    MedicoId = grupo.Key,
                    NomeMedico = grupo.First().Medico?.Nome ?? "Médico Desconhecido",
                    Consultas = grupo.Select(c => new ConsultaDetalhadaResponse
                    {
                        Id = c.Id,
                        NomePaciente = c.Paciente?.Nome ?? "Paciente Desconhecido",
                        DataHora = c.DataHora
                    }).ToList()
                }).ToList();

            return new ConsultaListaResponse
            {
                Medicos = medicos,
                TotalConsultas = totalConsultas,
                PaginaAtual = request.Page,
                TotalPaginas = (int)Math.Ceiling(totalConsultas / (double)request.Count)
            };
        }

        public async Task CancelarConsultaAsync(CancelConsultaRequest request)
        {
            var consulta = await _consultaRepository.GetByIdAsync(request.ConsultaId);
            if (consulta == null)
            {
                Console.WriteLine($"Consulta não encontrada: ID={request.ConsultaId}");
                throw new Exception("Consulta não encontrada.");
            }

            var agenda = await _agendaRepository.GetByIdAsync(consulta.AgendaId);
            if (agenda == null)
            {
                Console.WriteLine($"Erro: Agenda não encontrada para a consulta ID={consulta.Id}");
                throw new Exception("Erro ao recuperar a agenda da consulta.");
            }


            agenda.Disponivel = true;
            agenda.PacienteId = null;

            await _agendaRepository.UpdateAsync(agenda);
            await _consultaRepository.DeleteAsync(consulta.Id);

            var notificacao = new CreateNotificacaoRequest
            {
                UsuarioId = consulta.MedicoId,
                Mensagem = $"A consulta marcada para {consulta.DataHora} foi cancelada. Motivo: {request.Motivo}"
            };

            await _notificacaoService.EnviarNotificacaoAsync(consulta.MedicoId, notificacao.Mensagem);
            await _notificacaoService.EnviarNotificacaoAsync(consulta.PacienteId, notificacao.Mensagem);

            var paciente = await _pacienteRepository.FindByIdAsync(consulta.PacienteId);
            var medico = await _medicoRepository.GetByIdAsync(agenda.MedicoId);

            await _servicePublisherRabbit.PublishMessageAsync(new SendEmailCancelCommand(medico.Email, medico.Nome,
                paciente.Nome, DateTime.Now.ToString("d"), notificacao.Mensagem));

        }

        public async Task<PacienteHistoricoResponse> GetHistoricoPacienteAsync(int pacienteId)
        {
            var consultas = await _consultaRepository.GetHistoricoPacienteAsync(pacienteId);

            if (!consultas.Any())
            {
                return null;
            }

            var response = new PacienteHistoricoResponse
            {
                PacienteId = pacienteId,
                NomePaciente = consultas.First().Paciente?.Nome ?? "Paciente Desconhecido",
                Consultas = consultas.Select(c => new ConsultaDetalhadaPascienteResponse
                {
                    Id = c.Id,
                    NomeMedico = c.Medico?.Nome ?? "Médico Desconhecido",
                    DataHora = c.DataHora
                }).ToList()
            };

            return response;
        }

        public async Task<MedicoHistoricoResponse> GetHistoricoMedicoAsync(int medicoId)
        {
            var consultas = await _consultaRepository.GetHistoricoMedicoAsync(medicoId);

            if (!consultas.Any())
            {
                return null;
            }

            var response = new MedicoHistoricoResponse
            {
                MedicoId = medicoId,
                NomeMedico = consultas.First().Medico?.Nome ?? "Médico Desconhecido",
                Consultas = consultas.Select(c => new ConsultaDetalhadaMedicoResponse
                {
                    Id = c.Id,
                    NomePaciente = c.Paciente?.Nome ?? "Paciente Desconhecido",
                    DataHora = c.DataHora
                }).ToList()
            };

            return response;
        }

        public async Task<ConsultaResponse> AgendarConsultaAsync(CreateConsultaRequest request)
        {
            var agenda = await _agendaRepository.GetByIdAsync(request.AgendaId);
            var paciente = await _pacienteRepository.FindByIdAsync(request.PacienteId);
            var medico = await _medicoRepository.GetByIdAsync(agenda.MedicoId);
            
            if(medico is null || paciente is null)
                throw new Exception("Ids nao encontrados");

            if (agenda is not { Disponivel: true })
            {
                throw new Exception("O horário selecionado não está disponível.");
            }

            var consulta = new Consulta
            {
                PacienteId = request.PacienteId,
                MedicoId = agenda.MedicoId,
                AgendaId = request.AgendaId,
                DataHora = agenda.DataHora,
                Status = StatusConsulta.Pendente
            };

            agenda.Disponivel = false; 
            await _agendaRepository.UpdateAsync(agenda);

            await _consultaRepository.AddAsync(consulta);

            await _notificacaoService.EnviarNotificacaoAsync(consulta.MedicoId,
                $"Nova consulta pendente para {consulta.DataHora}. Acesse para aceitar ou recusar.");
            agenda.PacienteId = request.PacienteId;
            agenda.Disponivel = false;

            await _servicePublisherRabbit.PublishMessageAsync(new SendEmailAproveCommand(medico.Email, medico.Nome,
                paciente.Nome, DateTime.Now.ToString("d"), DateTime.Now.ToString("d")));

            return new ConsultaResponse
            {
                Id = consulta.Id,
                PacienteId = consulta.PacienteId,
                MedicoId = consulta.MedicoId,
                AgendaId = consulta.AgendaId,
                DataHora = consulta.DataHora,
                Status = consulta.Status.ToString()
            };
        }

        public async Task ResponderConsultaAsync(ResponderConsultaRequest request)
        {
            var consulta = await _consultaRepository.GetByIdAsync(request.ConsultaId);
            if (consulta == null)
            {
                throw new Exception("Consulta não encontrada.");
            }

            consulta.Status = request.Aceitar ? StatusConsulta.Confirmada : StatusConsulta.Recusada;
            await _consultaRepository.UpdateAsync(consulta);

            var agenda = await _agendaRepository.GetByIdAsync(consulta.AgendaId);
            if (agenda != null)
            {
                if (request.Aceitar)
                {
                    agenda.PacienteId = consulta.PacienteId;  
                    agenda.Disponivel = false;
                }
                else
                {
                    agenda.PacienteId = null;  
                    agenda.Disponivel = true;
                }

                await _agendaRepository.UpdateAsync(agenda);
            }
            
            string mensagem = request.Aceitar
                ? $"Sua consulta com o Dr. {consulta.Medico.Nome} foi confirmada para {consulta.DataHora}."
                : $"Sua consulta com o Dr. {consulta.Medico.Nome} foi recusada.";

            var paciente = await _pacienteRepository.FindByIdAsync(consulta.PacienteId);
            var medico = await _medicoRepository.GetByIdAsync(agenda.MedicoId);

            await _servicePublisherRabbit.PublishMessageAsync(new SendEmailCommand(medico.Email, medico.Nome,
                paciente.Nome, DateTime.Now.ToString("d"), DateTime.Now.ToString("d")));

            await _notificacaoService.EnviarNotificacaoAsync(consulta.PacienteId, mensagem);
           
        }

        public async Task<IEnumerable<ConsultaResponse>> GetPendentesPorMedicoAsync(int medicoId)
        {
            var consultas = await _consultaRepository.WhereAsync(c => c.MedicoId == medicoId && c.Status == StatusConsulta.Pendente);
            return _mapper.Map<IEnumerable<ConsultaResponse>>(consultas);
        }

        public async Task<IEnumerable<ConsultaResponse>> GetPendentesAsync(int medicoId)
        {
            var consultas = await _consultaRepository.GetPendentesByMedicoAsync(medicoId);
            return _mapper.Map<IEnumerable<ConsultaResponse>>(consultas);
        }
    }
}