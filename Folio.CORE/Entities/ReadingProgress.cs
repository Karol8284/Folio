using Folio.CORE.Domain.Entities;

namespace Folio.CORE.Entities
{
    public class ReadingProgress
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChapterId { get; set; }
        public int WordsRead { get; set; }
        public DateTime LastReadAt { get; set; }

        public User User { get; set; } = null;
        public Chapter Chapter { get; set; } = null;

    }
}
