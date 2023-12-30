using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Data;
using Register_Supply_Management.Model.Data;

namespace Register_Supply_Management.Repositories.Data
{
    public class AccountRepository : GeneralRepository<Account>, IAccountRepository
    {
        public AccountRepository(RegisterDBContext registerDbContext) : base(registerDbContext) { }

        public bool IsDuplicateValue(string value)
        {
            return _registerDbContext.Set<Account>().Any(ac => ac.PhoneNumber == value || ac.Email == value);
        }

        public Account? GetByEmail(string Email)
        {
            return _registerDbContext.Set<Account>().FirstOrDefault(ac => ac.Email == Email);
        }
    }
}
