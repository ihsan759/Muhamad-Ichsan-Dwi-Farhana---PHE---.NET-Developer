namespace Register_Supply_Management.Dtos.Project
{
    public class ProjectDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int VendorId { get; set; }
        public string NameVendor { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? BusinessFields { get; set; }
        public string? CompanyType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
