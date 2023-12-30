using System.ComponentModel.DataAnnotations.Schema;

namespace Register_Supply_Management.Model.Data
{
    public class Account : BaseEntity
    {
        [Column("email", TypeName = "varchar(255)")]
        public string Email { get; set; }
        [Column("name", TypeName = "varchar(255)")]
        public string Name { get; set; }
        [Column("password", TypeName = "varchar(255)")]
        public string Password { get; set; }
        [Column("phone_number", TypeName = "varchar(255)")]
        public string PhoneNumber { get; set; }
        [Column("image")]
        public string? Image { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("vendor_id")]
        public int? VendorId { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        //Cardinality
        public Role? Role { get; set; }
        public Vendor? Vendor { get; set; }
    }
}
