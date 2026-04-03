namespace Folio.Shared.DTOs.ReadingProgress
{
    public class ReadingProgressDto
    {
        /// <summary>Unique identifier for the reading progress record (primary key)</summary>
        public Guid Id { get; set; }

        /// <summary>Foreign key reference to the user who is reading (required)</summary>
        public Guid UserId { get; set; }

        /// <summary>Foreign key reference to the chapter being read (required)</summary>
        public Guid ChapterId { get; set; }

        /// <summary>Number of words read in this chapter by the user</summary>
        public int WordsRead { get; set; }

        /// <summary>Timestamp of the most recent reading session for this chapter in UTC</summary>
        public DateTime LastReadAt { get; set; }

        /// <summary>Navigation property - reference to the user tracking this progress</summary>
        public Folio.CORE.Entities.User User { get; set; } = null;

        /// <summary>Navigation property - reference to the chapter being tracked</summary>
        public Folio.CORE.Entities.Chapter Chapter { get; set; } = null;
    }
}
