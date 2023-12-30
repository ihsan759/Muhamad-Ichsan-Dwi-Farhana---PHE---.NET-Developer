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

        public AccountService(IAccountRepository accountRepository, IRoleRepository roleRepository, RegisterDBContext registerDBContext, ITokenHandler tokenHandler)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _registerDBContext = registerDBContext;
            _tokenHandler = tokenHandler;
        }

        public async Task<Account?> Register(RegisterDto registerDto)
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
                    Email = registerDto.Email,
                    Password = Hashing.HashPassword(registerDto.Password),
                    RoleId = 3,
                    Image = fileData,
                    PhoneNumber = registerDto.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                };

                var newAccount = _accountRepository.Create(account);
                transactions.Commit();
                return newAccount;
            }
            catch (Exception ex)
            {
                transactions.Rollback();
                return null;
            }
        }

        public string Login(LoginDto loginDto)
        {
            var getAccount = _accountRepository.GetByEmail(loginDto.Email);
            if (getAccount == null) return "0";

            if (!Hashing.ValidatePassword(loginDto.Password, getAccount!.Password)) return "0";

            if (getAccount.IsDeleted == true) return "1";

            try
            {
                var getRoleName = _roleRepository.GetById(getAccount.RoleId);

                var claims = new List<Claim>
                {
                    new Claim("Id", getAccount.Id.ToString()),
                    new Claim("Email", getAccount.Email),
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

        public int Approval(int id)
        {
            var getAccount = _accountRepository.GetById(id);
            if (getAccount == null) return 0;

            try
            {
                getAccount.RoleId = 4;
                getAccount.ModifiedAt = DateTime.Now;

                _accountRepository.Update(getAccount);

                return 1;
            }
            catch
            {
                return 2;
            }
        }

        public async Task<int> Update(UpdateAccountDto updateAccountDto, int userId)
        {

            var getAccount = _accountRepository.GetById(userId);
            if (getAccount is null) return 0;


            var transaction = _registerDBContext.Database.BeginTransaction();
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

                string fileData = null;

                string password = null;

                if (updateAccountDto.Password != null)
                {
                    Hashing.HashPassword(updateAccountDto.Password);
                }

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (updateAccountDto.Image != null && updateAccountDto.Image.Length != 0)
                {
                    if (getAccount.Image != null)
                    {
                        var oldImage = Path.Combine(filePath, getAccount.Image);
                        if (File.Exists(oldImage))
                        {
                            File.Delete(oldImage);
                        }
                    }
                    var extension = "." + updateAccountDto.Image.FileName.Split('.')[updateAccountDto.Image.FileName.Split('.').Length - 1];
                    fileData = DateTime.Now.Ticks.ToString() + extension;

                    var newPhoto = Path.Combine(filePath, fileData);
                    using (var stream = new FileStream(newPhoto, FileMode.Create))
                    {
                        await updateAccountDto.Image.CopyToAsync(stream);
                    }
                }

                var account = new Account
                {
                    Id = getAccount.Id,
                    Email = updateAccountDto.Email ?? getAccount.Email,
                    Name = updateAccountDto.Name ?? getAccount.Name,
                    Password = password ?? getAccount.Password,
                    Image = fileData ?? getAccount.Image,
                    RoleId = getAccount.RoleId,
                    PhoneNumber = updateAccountDto.PhoneNumber ?? getAccount.PhoneNumber,
                    VendorId = getAccount.VendorId,
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

        public IEnumerable<AccountDto>? GetAccountUser()
        {
            var getAccount = _accountRepository.GetAll().Where(ac => ac.RoleId == 3);
            if (!getAccount.Any()) return null;
            var dto = getAccount
                    .Select(ac => new AccountDto
                    {
                        Email = ac.Email,
                        Name = ac.Name,
                        PhoneNumber = ac.PhoneNumber,
                        RoleId = ac.RoleId
                    }).ToList();

            return dto;
        }

        public IEnumerable<AccountDto>? GetAccountVendor()
        {
            var getAccount = _accountRepository.GetAll().Where(ac => ac.RoleId == 4);
            if (!getAccount.Any()) return null;
            var dto = getAccount
                    .Select(ac => new AccountDto
                    {
                        Email = ac.Email,
                        Name = ac.Name,
                        PhoneNumber = ac.PhoneNumber,
                        RoleId = ac.RoleId
                    }).ToList();

            return dto;
        }

        public AccountDto? Get(int id)
        {
            var getAccount = _accountRepository.GetById(id);
            if (getAccount == null) return null;
            var dto = new AccountDto
            {
                Email = getAccount.Email,
                Name = getAccount.Name,
                PhoneNumber = getAccount.PhoneNumber,
                RoleId = getAccount.RoleId
            };

            return dto;
        }
    }
}
