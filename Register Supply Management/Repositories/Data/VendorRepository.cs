using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Data;
using Register_Supply_Management.Model.Data;

namespace Register_Supply_Management.Repositories.Data
{
    public class VendorRepository : GeneralRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(RegisterDBContext registerDbContext) : base(registerDbContext) { }

        public bool IsDuplicateValue(string value)
        {
            return _registerDbContext.Set<Vendor>().Any(ve => ve.Email == value || ve.PhoneNumber == value);
        }

        public Vendor? GetByAccount(int accountId)
        {
            var vendor = _registerDbContext.Set<Vendor>().FirstOrDefault(ve => ve.AccountId == accountId);
            _registerDbContext.ChangeTracker.Clear();
            return vendor;
        }

    }
}
