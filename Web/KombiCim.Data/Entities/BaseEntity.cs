using System.ComponentModel.DataAnnotations;

namespace Kombicim.Data.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; protected set; }
        public bool Active { get; set; }
    }
}
