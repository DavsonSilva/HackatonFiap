using AutoMapper;
using Hackaton.Domain.Entities.NotificacaoEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Domain.Requests.Notificacao;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;

namespace Hackaton.Infra.Services
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly INotificacaoRepository _notificacaoRepository;
        private readonly IMapper _mapper;

        public NotificacaoService(INotificacaoRepository notificacaoRepository, IMapper mapper)
        {
            _notificacaoRepository = notificacaoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificacaoResponse>> GetAllAsync()
        {
            var notificacoes = await _notificacaoRepository.AllAsync();
            return _mapper.Map<IEnumerable<NotificacaoResponse>>(notificacoes);
        }

        public async Task<NotificacaoResponse> GetByIdAsync(int id)
        {
            var notificacao = await _notificacaoRepository.FindByIdAsync(id);
            return _mapper.Map<NotificacaoResponse>(notificacao);
        }

        public async Task<IEnumerable<NotificacaoResponse>> GetByUsuarioIdAsync(int usuarioId)
        {
            var notificacoes = await _notificacaoRepository.GetByUsuarioIdAsync(usuarioId);
            return _mapper.Map<IEnumerable<NotificacaoResponse>>(notificacoes);
        }

        public async Task AddAsync(CreateNotificacaoRequest request)
        {
            var notificacao = _mapper.Map<Notificacao>(request);
            notificacao.DataEnvio = DateTime.UtcNow;

            await _notificacaoRepository.InsertAsync(notificacao);
        }

        public async Task DeleteAsync(int id)
        {
            await _notificacaoRepository.DeleteAsync(id);
        }

        public async Task EnviarNotificacaoAsync(int usuarioId, string mensagem)
        {
            var notificacao = new Notificacao
            {
                UsuarioId = usuarioId,
                Mensagem = mensagem,
                DataEnvio = DateTime.UtcNow
            };

            await _notificacaoRepository.InsertAsync(notificacao);
        }
    }
}
