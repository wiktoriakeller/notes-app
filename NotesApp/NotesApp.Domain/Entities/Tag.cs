namespace NotesApp.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string TagName { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
