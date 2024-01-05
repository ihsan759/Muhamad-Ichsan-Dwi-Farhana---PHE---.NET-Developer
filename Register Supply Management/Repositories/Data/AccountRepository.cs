using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Data;
using Register_Supply_Management.Model.Data;

namespace Register_Supply_Management.Repositories.Data
{
    public class AccountRepository : GeneralRepository<Account>, IAccountRepository
    {
        public AccountRepository(RegisterDBContext registerDbContext) : base(registerDbContext) { }

        public Account? GetByUsername(string Username)
        {
            return _registerDbContext.Set<Account>().FirstOrDefault(ac => ac.Username == Username);
        }
    }
}
