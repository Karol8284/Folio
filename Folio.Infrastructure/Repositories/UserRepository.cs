using Folio.CORE.Entities;
using Folio.CORE.Interfaces.Repositories;
using Folio.CORE.Responses;
using Folio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Folio.Infrastructure.Repositories
{
    /// <summary>
    /// UserRepository - obsługuje całą logikę dostępu do danych użytkowników
    /// Implementuje interfejs IUserRepository
    /// 
    /// Wzorzec: Repository Pattern
    /// - Oddziela logikę dostępu do danych od logiki biznesowej
    /// - Jedyne miejsce gdzie bezpośrednio pracujemy z DbContext
    /// </summary>
    public class UserRepository : IUserRepository
    {
        // Pole prywatne - dostęp do bazy danych
        // Wstrzykiwane przez Dependency Injection w Program.cs
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Konstruktor - otrzymuje DbContext przez Dependency Injection
        /// </summary>
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GetByIdAsync - pobiera użytkownika po ID
        /// Zwraca: Task<Result<User>>
        /// - Result.Success = znaleziono
        /// - Result.Failure = nie znaleziono
        /// </summary>
        public async Task<Result<User>> GetByIdAsync(Guid id)
        {
            try
            {
                // FirstOrDefaultAsync - zwraca pierwszy element lub null
                var user = await _context.Users
                    .AsNoTracking() // Optymalizacja - nie śledzmy zmian (tylko czytanie)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                    return Result<User>.Failure($"Użytkownik o ID {id} nie znaleziony");

                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Błąd podczas pobierania użytkownika: {ex.Message}");
            }
        }

        /// <summary>
        /// GetByEmailAsync - pobiera użytkownika po emailu
        /// Używane w LoginAsync i RegisterAsync
        /// </summary>
        public async Task<Result<User>> GetByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return Result<User>.Failure("Email nie może być pusty");

                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

                if (user == null)
                    return Result<User>.Failure($"Użytkownik o emailu {email} nie znaleziony");

                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Błąd podczas pobierania użytkownika: {ex.Message}");
            }
        }

        /// <summary>   
        /// GetAllAsync - pobiera WSZYSTKICH użytkowników
        /// ⚠️ UWAGA: W produkcji tego nie używaj dla dużych baz!
        /// Lepiej użyj GetPagedAsync
        /// </summary>
        public async Task<Result<List<User>>> GetAllAsync()
        {
            try
            {
                var users = await _context.Users
                    .AsNoTracking()
                    .ToListAsync();

                return Result<List<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return Result<List<User>>.Failure($"Błąd podczas pobierania użytkowników: {ex.Message}");
            }
        }

        /// <summary>
        /// GetPagedAsync - pobiera użytkowników ze stronicowaniem
        /// Przykład: str. 1, 10 elementów na stronę = elementy 0-9
        /// </summary>
        public async Task<Result<PagedResponse<User>>> GetPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                // Walidacja
                if (pageNumber < 1)
                    return Result<PagedResponse<User>>.Failure("Numer strony musi być >= 1");
                if (pageSize < 1)
                    return Result<PagedResponse<User>>.Failure("Rozmiar strony musi być >= 1");

                // Pobierz całkowitą liczbę rekordów
                var totalRecords = await _context.Users.CountAsync();

                // Skip = pomiń (pageNumber - 1) * pageSize
                // Take = pobierz pageSize elementów
                // Przykład: strona 2, size 10 -> Skip(10).Take(10) = elementy 10-19
                var users = await _context.Users
                    .AsNoTracking()
                    .OrderBy(u => u.CreatedAt) // Sortuj po dacie utworzenia
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var pagedResponse = new PagedResponse<User>(users, pageNumber, pageSize, totalRecords);
                return Result<PagedResponse<User>>.Success(pagedResponse);
            }
            catch (Exception ex)
            {
                return Result<PagedResponse<User>>.Failure($"Błąd podczas pobierania strony: {ex.Message}");
            }
        }

        /// <summary>
        /// CreateAsync - tworzy nowego użytkownika
        /// Zapisuje do bazy i zwraca nowego użytkownika z bazy
        /// </summary>  
        public async Task<Result<User>> CreateAsync(User user)
        {
            try
            {
                // Walidacja
                if (user == null)
                    return Result<User>.Failure("Użytkownik nie może być null");

                // Sprawdź czy email już istnieje
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email.ToLower() == user.Email.ToLower());

                if (emailExists)
                    return Result<User>.Failure($"Email {user.Email} już istnieje");

                // Dodaj do DbContext
                _context.Users.Add(user);

                // SaveChangesAsync - zapisuje zmiany do bazy danych
                // Wykonuje INSERT SQL
                await _context.SaveChangesAsync();

                return Result<User>.Success(user);
            }
            catch (DbUpdateException dbEx)
            {
                return Result<User>.Failure($"Błąd bazy danych: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Błąd podczas tworzenia użytkownika: {ex.Message}");
            }
        }

        /// <summary>
        /// UpdateAsync - aktualizuje istniejącego użytkownika
        /// </summary>
        public async Task<Result<User>> UpdateAsync(User user)
        {
            try
            {
                if (user == null)
                    return Result<User>.Failure("Użytkownik nie może być null");

                // Sprawdź czy użytkownik istnieje
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == user.Id);

                if (existingUser == null)
                    return Result<User>.Failure($"Użytkownik o ID {user.Id} nie znaleziony");

                // Update - zmienia dane istniejącego użytkownika
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Błąd podczas aktualizacji: {ex.Message}");
            }
        }

        /// <summary>
        /// DeleteAsync - usuwa użytkownika
        /// </summary>
        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                    return Result<bool>.Failure($"Użytkownik o ID {id} nie znaleziony");

                // Remove - usuwa z DbContext
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Błąd podczas usuwania: {ex.Message}");
            }
        }

        /// <summary>
        /// ExistsAsync - sprawdza czy użytkownik istnieje
        /// Zwraca true/false, nie całego użytkownika
        /// </summary>
        public async Task<Result<bool>> ExistsAsync(Guid id)
        {
            try
            {
                // AnyAsync - lepiej niż GetByIdAsync gdy potrzebujesz tylko true/false
                // Baza zoptymalizuje to zapytanie
                var exists = await _context.Users
                    .AnyAsync(u => u.Id == id);

                return Result<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Błąd podczas sprawdzania: {ex.Message}");
            }
        }

        /// <summary>
        /// EmailExistsAsync - sprawdza czy email istnieje
        /// Używane w RegisterAsync do walidacji
        /// </summary>
        public async Task<Result<bool>> EmailExistsAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return Result<bool>.Failure("Email nie może być pusty");

                var exists = await _context.Users
                    .AnyAsync(u => u.Email.ToLower() == email.ToLower());

                return Result<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Błąd podczas sprawdzania: {ex.Message}");
            }
        }
    }
}
