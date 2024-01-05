using FluentValidation;
using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Dtos.Project;

namespace Register_Supply_Management.Utilities.Validations
{
    public class ProjectCreateValidation : AbstractValidator<ProjectCreateDto>
    {

        private readonly IVendorRepository _vendorRepository;
        public ProjectCreateValidation(IVendorRepository vendorRepository)
        {

            _vendorRepository = vendorRepository;

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .Must((model, Name) => CheckApproval(model.Id)).WithMessage("vendor has not been approved");
        }

        public bool CheckApproval(string id)
        {
            var userId = int.Parse(id);
            var getVendor = _vendorRepository.GetByAccount(userId);
            if (getVendor == null) return false;

            if (getVendor.Approval == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
