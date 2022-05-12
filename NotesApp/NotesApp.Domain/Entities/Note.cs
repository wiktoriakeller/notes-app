namespace NotesApp.Domain.Entities
{
    public class Note : BaseEntity
    {
        public string NoteName { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string? HashId { get; set; }
        public int? HashIdSalt { get; set; }
        public DateTimeOffset? PublicLinkValidTill { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
