using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Register_Supply_Management.Model
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("modified_at")]
        public DateTime ModifiedAt { get; set; }
    }
}
