namespace Register_Supply_Management.Dtos.Vendor
{
    public class VendorUpdateDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? Image { get; set; }
        public string? BusinessFields { get; set; }
        public string? CompanyType { get; set; }
    }
}
