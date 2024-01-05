namespace Register_Supply_Management.Dtos.Account
{
    public class NewAccountDto
    {
        public string Name { get; set; }
        public string NameVendor { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
    }
}
