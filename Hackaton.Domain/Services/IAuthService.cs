using Hackaton.Domain.Requests.Auth;
using Hackaton.Domain.Responses;

namespace Hackaton.Domain.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> AuthenticateAsync(AuthLoginRequest request);
    }
}
