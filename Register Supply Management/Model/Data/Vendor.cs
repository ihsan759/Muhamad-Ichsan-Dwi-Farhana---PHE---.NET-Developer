using System.ComponentModel.DataAnnotations.Schema;

namespace Register_Supply_Management.Model.Data
{
    public class Vendor : BaseEntity
    {
        [Column("business_fields")]
        public string BusinessFields { get; set; }
        [Column("company_type")]
        public string CompanyType { get; set; }

        //Cardinality
        public ICollection<Account>? Accounts { get; set; }
    }
}
