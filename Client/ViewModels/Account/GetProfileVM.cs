namespace Client.ViewModels.Account
{
    public class GetProfileVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? rePassword { get; set; }
        public IFormFile? Image { get; set; }
    }
}
