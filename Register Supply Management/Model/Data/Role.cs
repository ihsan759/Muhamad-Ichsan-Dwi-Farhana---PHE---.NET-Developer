using System.ComponentModel.DataAnnotations.Schema;

namespace Register_Supply_Management.Model.Data
{
    public class Role : BaseEntity
    {
        [Column("name", TypeName = "varchar(100)")]
        public string Name { get; set; }

        //Cardinality
        public ICollection<Account>? Accounts { get; set; }
    }
}
