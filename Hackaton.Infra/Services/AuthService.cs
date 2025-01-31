using AutoMapper;
using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Entities.UsuarioEntity;
using Hackaton.Domain.Repositories;
using Hackaton.Domain.Requests.Auth;
using Hackaton.Domain.Responses;
using Hackaton.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hackaton.Infra.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<LoginResponse> AuthenticateAsync(AuthLoginRequest request)
        {
            var usuario = await _usuarioRepository.GetByEmailAndPasswordAsync(request.Email, request.Senha);
            if (usuario == null)
                throw new Exception("Usuário ou senha inválidos.");

            var token = GenerateJwtToken(usuario);

            return new LoginResponse
            {
                Token = token,
                Nome = usuario.Nome,
                Role = usuario is Medico ? "Médico" : "Paciente"
            };
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("role", usuario is Medico ? "Médico" : "Paciente")
            };

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
