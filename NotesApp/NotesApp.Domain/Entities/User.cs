namespace NotesApp.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

    }
}
