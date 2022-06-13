namespace NotesApp.Services.Dto
{
    public class NoteQuery
    {
        public string? SearchType { get; set; } = string.Empty;
        public string? SearchPhrase { get; set; } = string.Empty;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
