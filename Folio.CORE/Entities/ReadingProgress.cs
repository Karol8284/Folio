namespace Folio.CORE.Entities
{
    /// <summary>
    /// ReadingProgress - tracks user's reading activity and progress on specific chapters
    /// Stores reading statistics including words read and last read timestamp
    /// </summary>
    public class ReadingProgress
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
        public User User { get; set; } = null;

        /// <summary>Navigation property - reference to the chapter being tracked</summary>
        public Chapter Chapter { get; set; } = null;
    }
}
