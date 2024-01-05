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

            RuleFor(p => p.Username)
                .Must((model, username) => BeUniqueUsername(username, model.id)).WithMessage("'Username' already registered");

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

        }

        private bool BeUniqueUsername(string property, string id)
        {
            var userId = int.Parse(id);
            var account = _accountRepository.GetById(userId);
            if (account == null) return false;
            if (property == account.Username)
            {
                return true;
            }

            var username = _accountRepository.GetByUsername(property);

            if (username == null) return true;
            return false;
        }
    }
}
