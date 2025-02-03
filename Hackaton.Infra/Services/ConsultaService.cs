using AutoMapper;
using Hackaton.Domain.Entities.ConsultaEntity;
using Hackaton.Domain.Entities.PacienteEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Domain.Requests.Base;
using Hackaton.Domain.Requests.Consulta;
using Hackaton.Domain.Requests.Notificacao;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;
using Hackaton.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Hackaton.Infra.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IAgendaRepository _agendaRepository;
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISendGridService _sendGrid;

        public ConsultaService(IConsultaRepository consultaRepository, IAgendaRepository agendaRepository, INotificacaoService notificacaoService, IMapper mapper, IUsuarioRepository usuarioRepository, ISendGridService sendGrid)
        {
            _consultaRepository = consultaRepository;
            _agendaRepository = agendaRepository;
            _notificacaoService = notificacaoService;
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
            _sendGrid = sendGrid;
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
                throw new Exception("Consulta não encontrada.");
            }

            var agenda = await _agendaRepository.GetByIdAsync(consulta.AgendaId);
            if (agenda == null)
            {
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

            await _notificacaoService.AddAsync(notificacao);
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
            if (agenda == null || !agenda.Disponivel)
            {
                throw new Exception("O horário selecionado não está disponível.");
            }

            var paciente = await _usuarioRepository.GetByIdAsync(request.PacienteId);
            if (paciente == null)
            {
                throw new Exception("Paciente inválido.");
            }

            var consulta = new Consulta
            {
                PacienteId = request.PacienteId,
                MedicoId = agenda.MedicoId, 
                AgendaId = request.AgendaId,
                DataHora = agenda.DataHora
            };

            agenda.PacienteId = request.PacienteId;
            agenda.Disponivel = false;

            await _consultaRepository.AddAsync(consulta);
            await _agendaRepository.UpdateAsync(agenda);

            //arrumar o envio do e-mail aqi
            await _sendGrid.SendAppointmentNotificationAsync("davson1@hotmail.com", "Dr Davson", "Sly", DateTime.Now.ToString("d"), DateTime.Now.ToString("d"));

            //await _sendGrid.SendAppointmentNotificationAsync(consulta.Medico.Email, consulta.Medico.Nome, consulta.Paciente.Nome, DateTime.Now.ToString("d"), consulta.DataHora.ToString("d"));


            return new ConsultaResponse
            {
                Id = consulta.Id,
                PacienteId = consulta.PacienteId,
                MedicoId = consulta.MedicoId,
                AgendaId = consulta.AgendaId,
                DataHora = consulta.DataHora
            };
        }
    }
}
