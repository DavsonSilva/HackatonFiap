using AutoMapper;
using Hackaton.Domain.Entities.AgendaEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Domain.Requests.Agenda;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;

namespace Hackaton.Infra.Services
{
    public class AgendaService : IAgendaService
    {
        private readonly IAgendaRepository _agendaRepository;
        private readonly IMapper _mapper;

        public AgendaService(IAgendaRepository agendaRepository, IMapper mapper)
        {
            _agendaRepository = agendaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AgendaResponse>> GetAllAsync()
        {
            var agendas = await _agendaRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<AgendaResponse>>(agendas);
        }

        public async Task<AgendaResponse> GetByIdAsync(int id)
        {
            var agenda = await _agendaRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<AgendaResponse>(agenda);
        }

        public async Task AddAsync(CreateAgendaRequest request)
        {
            var agenda = _mapper.Map<Agenda>(request);
            await _agendaRepository.InsertAsync(agenda);
        }

        public async Task UpdateAsync(UpdateAgendaRequest request)
        {
            var agenda = _mapper.Map<Agenda>(request);
            await _agendaRepository.UpdateAsync(agenda);
        }

        public async Task DeleteAsync(int id)
        {
            await _agendaRepository.DeleteAsync(id);
        }
    }
}
