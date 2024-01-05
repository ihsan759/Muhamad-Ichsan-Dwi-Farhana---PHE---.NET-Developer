using System.ComponentModel.DataAnnotations.Schema;

namespace Register_Supply_Management.Model.Data
{
    public class Project : BaseEntity
    {
        [Column("name", TypeName = "varchar(255)")]
        public string Name { get; set; }
        [Column("status")]
        public bool Status { get; set; }
        [Column("vendor_id")]
        public int VendorId { get; set; }

        //Cardinality
        public Vendor? Vendor { get; set; }
    }
}
