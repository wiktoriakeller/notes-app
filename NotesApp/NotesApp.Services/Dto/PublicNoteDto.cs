namespace NotesApp.Services.Dto
{
    public class PublicNoteDto
    {
        public string NoteName { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTimeOffset PublicLinkValidTill { get; set; }
        public ICollection<TagDto> Tags { get; set; }
    }
}
