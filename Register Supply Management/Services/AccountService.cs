using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Contracts.Handler;
using Register_Supply_Management.Data;
using Register_Supply_Management.Dtos.Account;
using Register_Supply_Management.Model.Data;
using Register_Supply_Management.Utilities.Handlers;
using System.Security.Claims;

namespace Register_Supply_Management.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly RegisterDBContext _registerDBContext;
        private readonly ITokenHandler _tokenHandler;
        private readonly IVendorRepository _vendorRepository;

        public AccountService(IAccountRepository accountRepository, IRoleRepository roleRepository, RegisterDBContext registerDBContext, ITokenHandler tokenHandler, IVendorRepository vendorRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _registerDBContext = registerDBContext;
            _tokenHandler = tokenHandler;
            _vendorRepository = vendorRepository;
        }

        public async Task<NewAccountDto?> Register(RegisterDto registerDto)
        {
            using var transactions = _registerDBContext.Database.BeginTransaction();
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                var extension = "." + registerDto.Image.FileName.Split('.')[registerDto.Image.FileName.Split('.').Length - 1];
                var fileData = DateTime.Now.Ticks.ToString() + extension;

                var image = Path.Combine(filePath, fileData);
                using (var stream = new FileStream(image, FileMode.Create))
                {
                    await registerDto.Image.CopyToAsync(stream);
                }

                var account = new Account
                {
                    Name = registerDto.Name,
                    Username = registerDto.Username,
                    Password = Hashing.HashPassword(registerDto.Password),
                    RoleId = 3,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                };

                var newAccount = _accountRepository.Create(account);

                var vendor = new Vendor
                {
                    Name = registerDto.NameVendor,
                    Email = registerDto.Email,
                    Image = fileData,
                    PhoneNumber = registerDto.PhoneNumber,
                    Approval = false,
                    AccountId = newAccount.Id,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                };

                var newVendor = _vendorRepository.Create(vendor);

                var dto = new NewAccountDto
                {
                    Email = newVendor.Email,
                    Name = newAccount.Name,
                    NameVendor = newVendor.Name,
                    PhoneNumber = newVendor.PhoneNumber,
                    Username = newAccount.Username,
                    RoleId = newAccount.RoleId
                };
                transactions.Commit();
                return dto;
            }
            catch (Exception ex)
            {
                transactions.Rollback();
                return null;
            }
        }



        public string Login(LoginDto loginDto)
        {
            var getAccount = _accountRepository.GetByUsername(loginDto.Username);
            if (getAccount == null) return "0";

            if (!Hashing.ValidatePassword(loginDto.Password, getAccount!.Password)) return "0";

            if (getAccount.IsDeleted == true) return "1";

            try
            {
                var getRoleName = _roleRepository.GetById(getAccount.RoleId);

                var claims = new List<Claim>
                {
                    new Claim("Id", getAccount.Id.ToString()),
                    new Claim("Username", getAccount.Username),
                    new Claim(ClaimTypes.Name, getAccount.Name),
                    new Claim(ClaimTypes.Role, getRoleName.Name)
                };


                var getToken = _tokenHandler.GenerateToken(claims);
                return getToken;
            }
            catch
            {
                return "0";
            }

        }

        public int Update(UpdateAccountDto updateAccountDto, int userId)
        {

            var getAccount = _accountRepository.GetById(userId);
            if (getAccount is null) return 0;


            var transaction = _registerDBContext.Database.BeginTransaction();
            try
            {
                string password = null;

                if (updateAccountDto.Password != null)
                {
                    Hashing.HashPassword(updateAccountDto.Password);
                }

                var account = new Account
                {
                    Id = getAccount.Id,
                    Username = updateAccountDto.Username ?? getAccount.Username,
                    Name = updateAccountDto.Name ?? getAccount.Name,
                    Password = password ?? getAccount.Password,
                    RoleId = getAccount.RoleId,
                    CreatedAt = getAccount.CreatedAt,
                    ModifiedAt = DateTime.Now,
                    IsDeleted = getAccount.IsDeleted
                };

                _accountRepository.Update(account);
                transaction.Commit();
                return 1;
            }
            catch
            {
                transaction.Rollback();
                return 2;
            }
        }

        public int Delete(int id)
        {
            var getAccount = _accountRepository.GetById(id);
            if (getAccount == null) return 0;

            try
            {
                getAccount.IsDeleted = true;
                getAccount.ModifiedAt = DateTime.Now;

                _accountRepository.Update(getAccount);

                return 1;
            }
            catch
            {
                return 2;
            }
        }

        public AccountDto? Get(int id)
        {
            var getAccount = _accountRepository.GetById(id);
            if (getAccount == null) return null;
            var dto = new AccountDto
            {
                Username = getAccount.Username,
                Name = getAccount.Name,
            };

            return dto;
        }
    }
}
