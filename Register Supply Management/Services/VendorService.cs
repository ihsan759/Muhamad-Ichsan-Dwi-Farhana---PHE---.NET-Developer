using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Data;
using Register_Supply_Management.Dtos.Vendor;
using Register_Supply_Management.Model.Data;

namespace Register_Supply_Management.Services
{
    public class VendorService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly RegisterDBContext _registerDBContext;

        public VendorService(IAccountRepository accountRepository, RegisterDBContext registerDBContext, IVendorRepository vendorRepository)
        {
            _accountRepository = accountRepository;
            _registerDBContext = registerDBContext;
            _vendorRepository = vendorRepository;
        }

        public VendorDetailDto? Create(VendorCreateDto createDto, int id)
        {
            using var transactions = _registerDBContext.Database.BeginTransaction();
            try
            {
                var getAccount = _accountRepository.GetById(id);
                if (getAccount == null) return null;

                if (getAccount.RoleId != 4) return null;
                var vendor = new Vendor
                {
                    BusinessFields = createDto.BusinessFields,
                    CompanyType = createDto.CompanyType,
                };

                var newVendor = _vendorRepository.Create(vendor);
                getAccount.VendorId = newVendor.Id;

                var dto = new VendorDetailDto
                {
                    Name = getAccount.Name,
                    Email = getAccount.Email,
                    PhoneNumber = getAccount.PhoneNumber,
                    Image = getAccount.Image,
                    BusinessFields = createDto.BusinessFields,
                    CompanyType = createDto.CompanyType,
                };
                transactions.Commit();
                return dto;
            }
            catch
            {
                transactions.Rollback();
                return null;
            }
        }
    }
}
