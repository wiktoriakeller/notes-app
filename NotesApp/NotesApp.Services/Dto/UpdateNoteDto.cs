namespace NotesApp.Services.Dto
{
    public class UpdateNoteDto
    {
        public string NoteName { get; set; }
        public string Content { get; set; }
        public ICollection<TagDto> Tags { get; set; }
    }
}
