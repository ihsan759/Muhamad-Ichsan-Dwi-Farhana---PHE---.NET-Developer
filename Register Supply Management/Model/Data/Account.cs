using System.ComponentModel.DataAnnotations.Schema;

namespace Register_Supply_Management.Model.Data
{
    public class Account : BaseEntity
    {
        [Column("name", TypeName = "varchar(255)")]
        public string Name { get; set; }
        [Column("username", TypeName = "varchar(255)")]
        public string Username { get; set; }
        [Column("password", TypeName = "varchar(255)")]
        public string Password { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        //Cardinality
        public Role? Role { get; set; }
        public Vendor? Vendor { get; set; }
    }
}
