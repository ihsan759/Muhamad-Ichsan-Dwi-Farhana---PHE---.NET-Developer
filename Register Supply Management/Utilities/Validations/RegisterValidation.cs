using FluentValidation;
using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Dtos.Account;

namespace Register_Supply_Management.Utilities.Validations
{
    public class RegisterValidation : AbstractValidator<RegisterDto>
    {
        private readonly IAccountRepository _accountRepository;

        public RegisterValidation(IAccountRepository accountRepository)
        {

            _accountRepository = accountRepository;

            RuleFor(p => p.Email)
                .NotEmpty()
                .Must(BeUniqueProperty).WithMessage("'Email' already registered")
                .EmailAddress();

            RuleFor(p => p.Name)
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
                .NotNull().WithMessage("Image is required.")
                .DependentRules(() =>
                {
                    RuleFor(p => p.Image.Name)
                        .NotEmpty();

                    RuleFor(p => p.Image.ContentType)
                        .NotEmpty()
                        .Must(p => p.Equals("image/jpeg") || p.Equals("image/jpg") || p.Equals("image/png"))
                        .WithMessage("Invalid Image ContentType. Allowed types are JPEG, JPG, and PNG.");

                    RuleFor(p => p.Image.Length)
                        .NotNull()
                        .Must(length => length <= 2 * 1024 * 1024)
                        .WithMessage("Image size must be less than or equal to 10 MB.");
                });

        }

        private bool BeUniqueProperty(string property)
        {
            return !_accountRepository.IsDuplicateValue(property);
        }
    }
}
