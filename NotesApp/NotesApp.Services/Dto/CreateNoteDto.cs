namespace NotesApp.Services.Dto
{
    public class CreateNoteDto
    {
        public string NoteName { get; set; }
        public string Content { get; set; }
        public ICollection<string> Tags { get; set; }
    }
}
