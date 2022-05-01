namespace NotesApp.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string TagName { get; set; }
        public int NoteId { get; set; }
        public Note Note { get; set; }
    }
}
