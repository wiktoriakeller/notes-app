namespace NotesApp.Services.Dto
{
    public class NoteDto
    {
        public string HashId { get; set; }
        public string NoteName { get; set; }
        public string Content { get; set; }
        public string PublicHashId { get; set; }
        public DateTimeOffset PublicLinkValidTill { get; set; }
        public ICollection<TagDto> Tags { get; set; }
    }
}
