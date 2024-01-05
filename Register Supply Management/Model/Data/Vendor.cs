using System.ComponentModel.DataAnnotations.Schema;

namespace Register_Supply_Management.Model.Data
{
    public class Vendor : BaseEntity
    {
        [Column("email", TypeName = "varchar(255)")]
        public string Email { get; set; }
        [Column("name", TypeName = "varchar(255)")]
        public string Name { get; set; }
        [Column("phone_number", TypeName = "varchar(255)")]
        public string PhoneNumber { get; set; }
        [Column("image")]
        public string? Image { get; set; }
        [Column("business_fields")]
        public string? BusinessFields { get; set; }
        [Column("company_type")]
        public string? CompanyType { get; set; }
        [Column("approval")]
        public bool Approval { get; set; }
        [Column("account_id")]
        public int AccountId { get; set; }

        //Cardinality
        public Account? Account { get; set; }
        public ICollection<Project>? Projects { get; set; }
    }
}
