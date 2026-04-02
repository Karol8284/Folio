using Folio.CORE.Entities;
using Microsoft.EntityFrameworkCore;

namespace Folio.Infrastructure.Data
{
    /// <summary>
    /// ApplicationDbContext - główna klasa do komunikacji z bazą danych
    /// Dziedziczy z DbContext (EF Core)
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        // Te DbSet'y reprezentują tabele w bazie danych
        // DbSet<User> = tabela "Users" w SQL
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<ReadingProgress> ReadingProgresses { get; set; }

        /// <summary>
        /// OnModelCreating - tutaj definiujemy relacje między tabelami i konfiguracje
        /// Wykonywane raz przy starcie aplikacji
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracja relacji: Book -> Chapters (1 to Many)
            // Jeden Book może mieć wiele Chapters
            modelBuilder.Entity<Chapter>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Chapters)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade); // Jeśli usuniesz Book, usuną się Chapters

            // Konfiguracja relacji: User -> ReadingProgressRepository (1 to Many)
            modelBuilder.Entity<ReadingProgress>()
                .HasOne(rp => rp.User)
                .WithMany(u => u.ReadingProgress)
                .HasForeignKey(rp => rp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja relacji: ChapterRepository -> ReadingProgressRepository (1 to Many)
            modelBuilder.Entity<ReadingProgress>()
                .HasOne(rp => rp.Chapter)
                .WithMany(c => c.ReadingProgress)
                .HasForeignKey(rp => rp.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
