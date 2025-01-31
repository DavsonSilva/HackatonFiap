using AutoMapper;
using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Domain.Requests.Medico;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;

namespace Hackaton.Infra.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IMapper _mapper;

        public MedicoService(IMedicoRepository medicoRepository, IMapper mapper)
        {
            _medicoRepository = medicoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MedicoResponse>> GetAllAsync()
        {
            var medicos = await _medicoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MedicoResponse>>(medicos);
        }

        public async Task<MedicoResponse> GetByIdAsync(int id)
        {
            var medico = await _medicoRepository.GetByIdAsync(id);
            return _mapper.Map<MedicoResponse>(medico);
        }

        public async Task<IEnumerable<MedicoResponse>> SearchAsync(string query)
        {
            var medicos = await _medicoRepository.SearchAsync(query);
            return _mapper.Map<IEnumerable<MedicoResponse>>(medicos);
        }

        public async Task AddAsync(CreateMedicoRequest request)
        {
            var medico = _mapper.Map<Medico>(request);
            await _medicoRepository.AddAsync(medico);
        }

        public async Task UpdateAsync(UpdateMedicoRequest request)
        {
            var medico = _mapper.Map<Medico>(request);
            await _medicoRepository.UpdateAsync(medico);
        }

        public async Task DeleteAsync(int id)
        {
            await _medicoRepository.DeleteAsync(id);
        }
    }
}
