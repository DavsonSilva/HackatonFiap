using System.ComponentModel.DataAnnotations;

namespace Hackaton.Domain.Entities.BaseEntity
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
