namespace NotesApp.Services.Dto
{
    public class NoteDto
    {
        public int Id { get; set; }
        public string NoteName { get; set; }
        public string Content { get; set; }
        public ICollection<TagDto> Tags { get; set; }
    }
}
