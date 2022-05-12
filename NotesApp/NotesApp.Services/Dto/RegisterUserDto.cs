namespace NotesApp.Services.Dto
{
    public class RegisterUserDto
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
