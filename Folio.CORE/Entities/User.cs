using Folio.CORE.Enums;

namespace Folio.CORE.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public Role Role { get; set; }
        public bool isActive { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<ReadingProgress> ReadingProgress { get; set; } = new List<ReadingProgress>();
    }
}
