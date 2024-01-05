using Register_Supply_Management.Model.Data;

namespace Register_Supply_Management.Contracts.Data
{
    public interface IVendorRepository : IGeneralRepository<Vendor>
    {
        bool IsDuplicateValue(string value);
        Vendor? GetByAccount(int accountId);
    }
}
