using System;
using System.Collections.Generic;
using System.Text;

namespace Folio.CORE.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(string email, string password, string fullName);
        Task<(bool Success, string Token, string Message)> LoginAsync(string email, string password);
        Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    }
}
