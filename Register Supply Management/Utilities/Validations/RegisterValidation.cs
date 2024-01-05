using FluentValidation;
using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Dtos.Account;

namespace Register_Supply_Management.Utilities.Validations
{
    public class RegisterValidation : AbstractValidator<RegisterDto>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IVendorRepository _vendorRepository;

        public RegisterValidation(IAccountRepository accountRepository, IVendorRepository vendorRepository)
        {

            _accountRepository = accountRepository;
            _vendorRepository = vendorRepository;

            RuleFor(p => p.Username)
                .NotEmpty()
                .Must(BeUniqueUsername).WithMessage("'Username' already registered");

            RuleFor(p => p.Email)
                .NotEmpty()
                .Must(BeUniqueProperty).WithMessage("'Email' already registered")
                .EmailAddress();

            RuleFor(p => p.Name)
                .NotEmpty();

            RuleFor(p => p.NameVendor)
                .NotEmpty();

            RuleFor(p => p.Password)
                .NotEmpty()
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(p => p.rePassword)
                .NotEmpty()
                .Equal(model => model.Password).WithMessage("Password and Confirm Password do not match.");

            RuleFor(p => p.PhoneNumber)
                .NotEmpty()
                .Must(BeUniqueProperty).WithMessage("'Phone Number' already registered");

            RuleFor(p => p.Image)
                .NotNull()
                .Must(ValidateImage).WithMessage("Invalid photo extension. Only JPG and JPEG files are allowed.");

        }

        private bool BeUniqueProperty(string property)
        {
            return !_vendorRepository.IsDuplicateValue(property);
        }

        private bool BeUniqueUsername(string username)
        {
            var unique = _accountRepository.GetByUsername(username);

            if (unique == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool ValidateImage(IFormFile file)
        {
            if (file is null)
            {
                return true;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(fileExtension);
        }
    }
}
