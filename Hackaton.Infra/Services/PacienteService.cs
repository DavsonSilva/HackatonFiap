using AutoMapper;
using Hackaton.Domain.Entities.PacienteEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Domain.Requests.Paciente;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;

namespace Hackaton.Infra.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMapper _mapper;

        public PacienteService(IPacienteRepository pacienteRepository, IMapper mapper)
        {
            _pacienteRepository = pacienteRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PacienteResponse>> GetAllAsync()
        {
            var pacientes = await _pacienteRepository.AllAsync();
            return _mapper.Map<IEnumerable<PacienteResponse>>(pacientes);
        }

        public async Task<PacienteResponse> GetByIdAsync(int id)
        {
            var paciente = await _pacienteRepository.FindByIdAsync(id);
            return _mapper.Map<PacienteResponse>(paciente);
        }

        public async Task AddAsync(CreatePacienteRequest request)
        {
            var paciente = _mapper.Map<Paciente>(request);
            await _pacienteRepository.InsertAsync(paciente);
        }

        public async Task UpdateAsync(UpdatePacienteRequest request)
        {
            var paciente = _mapper.Map<Paciente>(request);
            await _pacienteRepository.UpdateAsync(paciente);
        }

        public async Task DeleteAsync(int id)
        {
            await _pacienteRepository.DeleteAsync(id);
        }
    }
}
