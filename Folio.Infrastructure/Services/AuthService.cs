using BCrypt.Net;
using Folio.CORE.Entities;
using Folio.CORE.Interfaces;
using Folio.CORE.Interfaces.Repositories;
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
            if (existingUser.IsSuccess && existingUser.Value != null)
                return (false, "Użytkownik z takim emailem już istnieje");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = passwordHash,
                DisplayName = fullName,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userRepository.CreateAsync(user);
            if (!result.IsSuccess)
                return (false, result.Error ?? "Błąd podczas rejestracji");

            return (true, "Rejestracja pomyślna");
        }

        public async Task<(bool Success, string Token, string Message)> LoginAsync(
            string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, "", "Email i hasło są wymagane");

            var userResult = await _userRepository.GetByEmailAsync(email);
            if (!userResult.IsSuccess || userResult.Value == null)
                return (false, "", "Nieprawidłowe dane logowania");

            var user = userResult.Value;
            if (!user.isActive)
                return (false, "", "Konto jest wyłączone");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return (false, "", "Nieprawidłowe dane logowania");

            var token = _jwtTokenProvider.GenerateToken(user.Id.GetHashCode(), user.Email, user.DisplayName);
            return (true, token, "Zalogowano pomyślnie");
        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(
            int userId, string oldPassword, string newPassword)
        {
            // Konwersja z int na Guid - nieidealna, ale zachowuje kompatybilność
            var userGuid = new Guid(userId.ToString("D").PadRight(36, '0'));
            var userResult = await _userRepository.GetByIdAsync(userGuid);

            if (!userResult.IsSuccess || userResult.Value == null)
                return (false, "Użytkownik nie znaleziony");

            var user = userResult.Value;
            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
                return (false, "Stare hasło jest nieprawidłowe");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            var updateResult = await _userRepository.UpdateAsync(user);

            if (!updateResult.IsSuccess)
                return (false, updateResult.Error ?? "Błąd podczas zmiany hasła");

            return (true, "Hasło zmienione pomyślnie");
        }
    }
}