using Hackaton.Domain.Requests.Auth;
using Hackaton.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginUser([FromBody] AuthUserLoginRequest request)
        {
            var response = await _authService.AuthenticateAsync(request);
            return Ok(response);
        }

        [HttpPost("loginMedic")]
        public async Task<IActionResult> LoginMedic([FromBody] AuthMedicLoginRequest request)
        {
            var response = await _authService.AuthenticateMedicAsync(request);
            return Ok(response);
        }
    }
}
