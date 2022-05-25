namespace NotesApp.Services.Dto
{
    public class CreateNoteDto
    {
        public string NoteName { get; set; }
        public string Content { get; set; }
        public string? ImageLink { get; set; }
        public ICollection<CreateTagDto> Tags { get; set; }
    }
}
