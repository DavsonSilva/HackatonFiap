using Hackaton.Domain.Entities.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace Hackaton.Domain.Entities.UsuarioEntity
{
    public class Usuario : Entity
    {
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Senha { get; set; }
    }
}
