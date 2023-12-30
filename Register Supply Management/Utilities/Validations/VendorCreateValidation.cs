using FluentValidation;
using Register_Supply_Management.Dtos.Vendor;

namespace Register_Supply_Management.Utilities.Validations
{
    public class VendorCreateValidation : AbstractValidator<VendorCreateDto>
    {
        public VendorCreateValidation()
        {
            RuleFor(p => p.BusinessFields)
                .NotEmpty();

            RuleFor(p => p.CompanyType)
                .NotEmpty();
        }
    }
}
