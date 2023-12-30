using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Data;
using Register_Supply_Management.Model.Data;

namespace Register_Supply_Management.Repositories.Data
{
    public class VendorRepository : GeneralRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(RegisterDBContext registerDbContext) : base(registerDbContext) { }
    }
}
