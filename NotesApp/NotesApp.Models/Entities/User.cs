namespace NotesApp.Models.Entities
{
    public class User : BaseEntity
    {
        public string Login { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
