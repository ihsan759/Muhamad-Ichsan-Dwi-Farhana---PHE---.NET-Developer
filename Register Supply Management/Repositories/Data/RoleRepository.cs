using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Data;
using Register_Supply_Management.Model.Data;

namespace Register_Supply_Management.Repositories.Data
{
    public class RoleRepository : GeneralRepository<Role>, IRoleRepository
    {
        public RoleRepository(RegisterDBContext registerDbContext) : base(registerDbContext) { }
    }
}
