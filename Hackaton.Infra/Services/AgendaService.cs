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
    }
}
