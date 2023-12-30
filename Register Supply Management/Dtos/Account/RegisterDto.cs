namespace Register_Supply_Management.Dtos.Account
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string rePassword { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile Image { get; set; }
    }
}
