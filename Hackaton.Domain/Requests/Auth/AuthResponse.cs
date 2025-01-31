namespace Hackaton.Domain.Requests.Auth
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
