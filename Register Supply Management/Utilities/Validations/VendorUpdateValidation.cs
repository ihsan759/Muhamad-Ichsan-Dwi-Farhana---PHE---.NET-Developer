using FluentValidation;
using Register_Supply_Management.Contracts.Data;
using Register_Supply_Management.Dtos.Vendor;

namespace Register_Supply_Management.Utilities.Validations
{
    public class VendorUpdateValidation : AbstractValidator<VendorUpdateDto>
    {
        private readonly IVendorRepository _vendorRepository;
        public VendorUpdateValidation(IVendorRepository vendorRepository)
        {

            _vendorRepository = vendorRepository;

            RuleFor(p => p.Email)
                .Must((model, email) => BeUniqueProperty(email, model.Id)).WithMessage("'Email' already registered")
                .EmailAddress()
                .When(p => !string.IsNullOrEmpty(p.Email));

            RuleFor(p => p.PhoneNumber)
                .Must((model, phoneNumber) => BeUniqueProperty(phoneNumber, model.Id)).WithMessage("'Phone Number' already registered")
                .When(p => !string.IsNullOrEmpty(p.PhoneNumber));

            RuleFor(p => p.Image)
                .Must(ValidateImage).WithMessage("Invalid photo extension. Only JPG and JPEG files are allowed.");
        }

        private bool BeUniqueProperty(string property, string id)
        {
            var userId = int.Parse(id);
            var vendor = _vendorRepository.GetByAccount(userId);
            if (vendor == null) return false;
            if (property == vendor.Email || property == vendor.PhoneNumber)
            {
                return true;
            }
            return !_vendorRepository.IsDuplicateValue(property);
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
