using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Hackaton.Domain.Entities.UsuarioEntity;
using Hackaton.Domain.Entities.BaseEntity;

namespace Hackaton.Domain.Entities.NotificacaoEntity
{
    public class Notificacao : Entity
    {
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        [Required]
        public string Mensagem { get; set; }

        public DateTime DataEnvio { get; set; } = DateTime.UtcNow;
    }
}
