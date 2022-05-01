namespace NotesApp.Domain.Entities
{
    public class Note : BaseEntity
    {
        public string NoteName { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
