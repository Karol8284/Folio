using Folio.CORE.Domain.Entities;
using Folio.CORE.Interfaces;
using Folio.Infrastructure.Security;

namespace Folio.Infrastructure.Services
{
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtTokenProvider _jwtTokenProvider;

    public AuthService(IUserRepository userRepository, JwtTokenProvider jwtTokenProvider)
    {
        _userRepository = userRepository;
        _jwtTokenProvider = jwtTokenProvider;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(
        string email, string password, string fullName)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return (false, "Email i hasło są wymagane");

        var existingUser = await _userRepository.GetByEmailAsync(email);
        if (existingUser != null)
            return (false, "Użytkownik z takim emailem już istnieje");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Email = email,
            PasswordHash = passwordHash,
            FullName = fullName,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return (true, "Rejestracja успешna");
    }

    public async Task<(bool Success, string Token, string Message)> LoginAsync(
        string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return (false, "", "Email i hasło są wymagane");

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || !user.IsActive)
            return (false, "", "Nieprawidłowe dane logowania");

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return (false, "", "Nieprawidłowe dane logowania");

        var token = _jwtTokenProvider.GenerateToken(user.Id, user.Email, user.FullName);
        return (true, token, "Zalogowano pomyślnie");
    }

    public async Task<(bool Success, string Message)> ChangePasswordAsync(
        int userId, string oldPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return (false, "Użytkownik nie znaleziony");

        if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
            return (false, "Stare hasło jest nieprawidłowe");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _userRepository.SaveChangesAsync();

        return (true, "Hasło zmienione pomyślnie");
    }
}