using AutoMapper;
using Hackaton.Domain.Entities.MedicoEntity;
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
        private readonly IPasswordHasherService _passwordHasherService;

        public PacienteService(IPacienteRepository pacienteRepository, IMapper mapper, IPasswordHasherService passwordHasherService)
        {
            _pacienteRepository = pacienteRepository;
            _mapper = mapper;
            _passwordHasherService = passwordHasherService;
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

        public async Task<PacienteResponse> AddAsync(CreatePacienteRequest request)
        {
            var existe = await _pacienteRepository.FindByEmailOrCPFAsync(request.Email, request.CPF);
            if (existe != null)
            {
                throw new Exception("CPF ou Email já cadastrado.");
            }

            var paciente = _mapper.Map<Paciente>(request);

            paciente.Senha = _passwordHasherService.HashPassword(request.Senha);
            if (!_passwordHasherService.VerifyPassword(request.Senha, paciente.Senha))
            {
                throw new Exception("Senha inválida.");
            }

            await _pacienteRepository.InsertAsync(paciente);

            return _mapper.Map<PacienteResponse>(paciente);
        }

        public async Task UpdateAsync(UpdatePacienteRequest request)
        {
            var paciente = await _pacienteRepository.FindByIdAsync(request.Id);
            if (paciente == null)
            {
                throw new Exception("Paciente não encontrado.");
            }

            paciente.Nome = request.Nome;
            paciente.CPF = request.CPF;
            paciente.Email = request.Email;

            if (!string.IsNullOrEmpty(request.Senha))
            {
                paciente.Senha = _passwordHasherService.HashPassword(request.Senha);
            }

            await _pacienteRepository.UpdateAsync(paciente);
        }

        public async Task DeleteAsync(int id)
        {
            await _pacienteRepository.DeleteAsync(id);
        }
    }
}
