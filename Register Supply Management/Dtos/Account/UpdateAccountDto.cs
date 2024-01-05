namespace Register_Supply_Management.Dtos.Account
{
    public class UpdateAccountDto
    {
        public string id { get; set; }
        public string Username { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? rePassword { get; set; }
    }
}
