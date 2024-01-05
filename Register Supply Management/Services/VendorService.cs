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

        public async Task<int> Update(VendorUpdateDto vendorUpdateDto, int userId)
        {
            var getVendor = _vendorRepository.GetByAccount(userId);
            if (getVendor == null) return 0;


            var transaction = _registerDBContext.Database.BeginTransaction();
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

                string fileData = null;

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (vendorUpdateDto.Image != null && vendorUpdateDto.Image.Length != 0)
                {
                    if (getVendor.Image != null)
                    {
                        var oldImage = Path.Combine(filePath, getVendor.Image);
                        if (File.Exists(oldImage))
                        {
                            File.Delete(oldImage);
                        }
                    }
                    var extension = "." + vendorUpdateDto.Image.FileName.Split('.')[vendorUpdateDto.Image.FileName.Split('.').Length - 1];
                    fileData = DateTime.Now.Ticks.ToString() + extension;

                    var newPhoto = Path.Combine(filePath, fileData);
                    using (var stream = new FileStream(newPhoto, FileMode.Create))
                    {
                        await vendorUpdateDto.Image.CopyToAsync(stream);
                    }
                }

                var vendor = new Vendor
                {
                    Id = getVendor.Id,
                    Email = vendorUpdateDto.Email ?? getVendor.Email,
                    Name = vendorUpdateDto.Name ?? getVendor.Name,
                    Image = fileData ?? getVendor.Image,
                    PhoneNumber = vendorUpdateDto.PhoneNumber ?? getVendor.PhoneNumber,
                    BusinessFields = getVendor.BusinessFields,
                    CompanyType = getVendor.CompanyType,
                    Approval = getVendor.Approval,
                    AccountId = getVendor.AccountId,
                    CreatedAt = getVendor.CreatedAt,
                    ModifiedAt = DateTime.Now,
                };

                if (getVendor.Approval == true)
                {
                    vendor.BusinessFields = vendorUpdateDto.BusinessFields;
                    vendor.CompanyType = vendorUpdateDto.CompanyType;
                }

                _vendorRepository.Update(vendor);
                transaction.Commit();
                return 1;
            }
            catch
            {
                transaction.Rollback();
                return 2;
            }
        }

        public VendorDto Get(int userId)
        {
            var getVendor = _vendorRepository.GetByAccount(userId);
            if (getVendor == null) return null;

            var dto = new VendorDto
            {
                Id = getVendor.Id,
                Name = getVendor.Name,
                Email = getVendor.Email,
                Image = getVendor.Image,
                PhoneNumber = getVendor.PhoneNumber,
                BusinessFields = getVendor.BusinessFields,
                CompanyType = getVendor.CompanyType,
            };

            return dto;
        }

        public IEnumerable<VendorDto> GetAll()
        {
            var vendors = _vendorRepository.GetAll().Where(ve => ve.Approval == true);
            if (!vendors.Any()) return null;

            var dto = (from vendor in vendors
                       select new VendorDto
                       {
                           Id = vendor.Id,
                           Name = vendor.Name,
                           Email = vendor.Email,
                           Image = vendor.Image,
                           PhoneNumber = vendor.PhoneNumber,
                           BusinessFields = vendor.BusinessFields,
                           CompanyType = vendor.CompanyType,
                       }).ToList();

            return dto;
        }

        public IEnumerable<VendorDto> GetAdmin()
        {
            var vendors = _vendorRepository.GetAll().Where(ve => ve.Approval != true);
            if (!vendors.Any()) return null;

            var dto = (from vendor in vendors
                       select new VendorDto
                       {
                           Id = vendor.Id,
                           Name = vendor.Name,
                           Email = vendor.Email,
                           Image = vendor.Image,
                           PhoneNumber = vendor.PhoneNumber,
                           BusinessFields = vendor.BusinessFields,
                           CompanyType = vendor.CompanyType,
                       }).ToList();

            return dto;
        }

        public int Approval(int id)
        {
            var getVendor = _vendorRepository.GetById(id);
            if (getVendor == null) return 0;

            try
            {
                getVendor.Approval = true;
                getVendor.ModifiedAt = DateTime.Now;

                _vendorRepository.Update(getVendor);

                return 1;
            }
            catch
            {
                return 2;
            }
        }
    }
}
