using AutoMapper;
using Hackaton.Domain.Entities.AgendaEntity;
using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Domain.Requests.Agenda;
using Hackaton.Domain.Requests.Medico;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;

namespace Hackaton.Infra.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IAgendaRepository _agendaRepository;
        private readonly IPasswordHasherService _passwordHasherService;

        private readonly IMapper _mapper;

        public MedicoService(IMedicoRepository medicoRepository, IMapper mapper, IAgendaRepository agendaRepository, IPasswordHasherService passwordHasherService)
        {
            _medicoRepository = medicoRepository;
            _mapper = mapper;
            _agendaRepository = agendaRepository;
            _passwordHasherService = passwordHasherService;
        }

        public async Task<IEnumerable<MedicoResponse>> GetAllAsync()
        {
            var medicos = await _medicoRepository.GetAllAsync();

            var response = medicos.Select(medico => new MedicoResponse
            {
                Id = medico.Id,
                Nome = medico.Nome,
                CRM = medico.CRM,
                Email = medico.Email,
                HorariosDisponiveis = medico.HorariosDisponiveis
                    .Select(h => new AgendaResponse
                    {
                        Id = h.Id,
                        MedicoId = medico.Id,
                        MedicoNome = medico.Nome,
                        PacienteId = h.PacienteId,
                        PacienteNome = h.PacienteId.HasValue ? h.Paciente?.Nome : null,
                        DataHora = h.DataHora,
                        Disponivel = h.Disponivel
                    })
                    .ToList()
            }).ToList();

            return response;
        }

        public async Task<MedicoResponse> GetByIdAsync(int id)
        {
            var medico = await _medicoRepository.GetByIdAsync(id);
            if (medico == null)
                return null;

            return new MedicoResponse
            {
                Id = medico.Id,
                Nome = medico.Nome,
                CRM = medico.CRM,
                Email = medico.Email,
                HorariosDisponiveis = medico.HorariosDisponiveis
                    .Select(h => new AgendaResponse
                    {
                        Id = h.Id,
                        MedicoId = medico.Id,
                        MedicoNome = medico.Nome,
                        PacienteId = h.PacienteId,
                        PacienteNome = h.PacienteId.HasValue ? h.Paciente?.Nome : null,
                        DataHora = h.DataHora,
                        Disponivel = h.Disponivel
                    })
                    .ToList()
            };
        }

        public async Task<MedicoResponse> AddAsync(CreateMedicoRequest request)
        {
            var medico = _mapper.Map<Medico>(request);

            medico.Senha = _passwordHasherService.HashPassword(request.Senha);
            if (!_passwordHasherService.VerifyPassword(request.Senha, medico.Senha))
            {
                throw new Exception("Senha inválida.");
            }

            await _medicoRepository.AddAsync(medico);

            var medicoSalvo = await _medicoRepository.GetByIdAsync(medico.Id);

            return _mapper.Map<MedicoResponse>(medicoSalvo);
        }

        public async Task UpdateAsync(UpdateMedicoRequest request)
        {
            var medico = await _medicoRepository.GetByIdAsync(request.Id);
            if (medico == null)
            {
                throw new Exception("Médico não encontrado");
            }

            medico.Nome = request.Nome;
            medico.CRM = request.CRM;
            medico.Email = request.Email;

            if (!string.IsNullOrEmpty(request.Senha))
            {
                medico.Senha = _passwordHasherService.HashPassword(request.Senha);
            }

            await _medicoRepository.UpdateAsync(medico);
        }

        public async Task DeleteAsync(int id)
        {
            await _medicoRepository.DeleteAsync(id);
        }


        //Agenda
        public async Task AddAgendaAsync(int medicoId, List<CreateAgendaRequest> agendaRequests)
        {
            var medico = await _medicoRepository.GetByIdAsync(medicoId);
            if (medico == null)
            {
                throw new Exception("Médico não encontrado");
            }

            if (medico.HorariosDisponiveis == null)
            {
                medico.HorariosDisponiveis = new List<Agenda>();
            }

            var agendas = agendaRequests.Select(a => new Agenda
            {
                Id = 0, 
                DataHora = DateTime.SpecifyKind(a.DataHora, DateTimeKind.Utc),
                Disponivel = a.Disponivel,
                Medico = medico
            }).ToList();

            medico.HorariosDisponiveis.AddRange(agendas);

            await _medicoRepository.UpdateAsync(medico);
        }

        public async Task EditarAgendaAsync(int medicoId, int agendaId, UpdateAgendaRequest request)
        {
            var agenda = await _agendaRepository.GetByIdAsync(agendaId);

            if (agenda == null || agenda.MedicoId != medicoId)
            {
                throw new Exception("Agenda não encontrada ou não pertence ao médico.");
            }

            agenda.DataHora = request.DataHora;
            agenda.Disponivel = request.Disponivel;

            await _agendaRepository.UpdateAsync(agenda);
        }

        public async Task ExcluirAgendaAsync(int medicoId, int agendaId)
        {
            var medico = await _medicoRepository.GetByIdAsync(medicoId);
            if (medico == null)
                throw new Exception("Médico não encontrado.");

            var agenda = medico.HorariosDisponiveis.FirstOrDefault(a => a.Id == agendaId);
            if (agenda == null)
                throw new Exception("Horário não encontrado.");

            await _agendaRepository.DeleteAgendaAsync(agendaId);
        }
    }
}
