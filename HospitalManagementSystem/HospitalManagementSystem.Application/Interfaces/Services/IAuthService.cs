using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;


namespace HospitalManagementSystem.Application.Interfaces.Services;

public interface IAuthService
{
    Task<User?> LoginAsync(string username, string password);
    Task<(bool Success, string? ErrorMessage)> RegisterAsync(string username, string password, UserRole role);
    User? GetCurrentUser();
}
