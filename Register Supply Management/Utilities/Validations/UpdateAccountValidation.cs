using FluentValidation;
using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Dtos.Account;

namespace Register_Supply_Management.Utilities.Validations
{
    public class UpdateAccountValidation : AbstractValidator<UpdateAccountDto>
    {
        private readonly IAccountRepository _accountRepository;

        public UpdateAccountValidation(IAccountRepository accountRepository)
        {

            _accountRepository = accountRepository;

            RuleFor(p => p.id)
                .NotNull();

            RuleFor(p => p.Email)
                .Must((model, email) => BeUniqueProperty(email, model.id)).WithMessage("'Email' already registered")
                .EmailAddress()
                .When(p => !string.IsNullOrEmpty(p.Email));

            RuleFor(p => p.Password)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.")
                .When(p => !string.IsNullOrEmpty(p.Password));

            RuleFor(p => p.rePassword)
                .NotNull()
                .Equal(model => model.Password).WithMessage("Password and Confirm Password do not match.")
                .When(p => !string.IsNullOrEmpty(p.Password));

            RuleFor(p => p.PhoneNumber)
                .Must((model, phoneNumber) => BeUniqueProperty(phoneNumber, model.id)).WithMessage("'Phone Number' already registered")
                .When(p => !string.IsNullOrEmpty(p.PhoneNumber));

            RuleFor(p => p.Image)
                .Must(ValidateImage).WithMessage("Invalid photo extension. Only JPG and JPEG files are allowed.");

        }

        private bool BeUniqueProperty(string property, string id)
        {
            var userId = int.Parse(id);
            var account = _accountRepository.GetById(userId);
            if (property == account.Email || property == account.PhoneNumber)
            {
                return true;
            }
            return !_accountRepository.IsDuplicateValue(property);
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
