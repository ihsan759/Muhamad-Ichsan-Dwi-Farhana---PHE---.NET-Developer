using Register_Supply_Management.Model.Data;

namespace Register_Supply_Management.Contracts.Data
{
    public interface IAccountRepository : IGeneralRepository<Account>
    {
        public bool IsDuplicateValue(string value);
        public Account? GetByEmail(string Email);
    }
}
