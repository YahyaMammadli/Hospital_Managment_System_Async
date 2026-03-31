

using HospitalManagementSystem.Application.Interfaces.Repositories;
using HospitalManagementSystem.Application.Interfaces.Services;
using HospitalManagementSystem.Application.Interfaces.Validators;
using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;
using System.Security.Cryptography;
using System.Text;

namespace HospitalManagementSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserValidator _userValidator;
    private User? _currentUser;

    public AuthService(IUserRepository userRepository, IUserValidator userValidator)
    {
        _userRepository = userRepository;
        _userValidator = userValidator;
    }




    public async Task<User?> LoginAsync(string username, string password)
    {
        if (!_userValidator.ValidateUsername(username) || !_userValidator.ValidatePassword(password))
            return null;

        var user = await _userRepository.GetByUsernameAsync(username);
        
        if (user == null)
            return null;

        var passwordHash = HashPassword(password,user.Salt);

        if (passwordHash != user.PasswordHash)
            return null;

        _currentUser = user;


        return user;
    
    
    }






    public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(string username, string password, UserRole role)
    {
        if (!_userValidator.ValidateUsername(username) || !_userValidator.ValidatePassword(password))
            return (false, "Invalid data\n\n");

        var existing = await _userRepository.GetByUsernameAsync(username);

        if (existing != null)
            return (false, "Username already exists\n\n");


        var salt = GenerateSalt();

        var passwordHash = HashPassword(password, salt);

        var user = new User
        {
            Username = username,
            PasswordHash = passwordHash,
            Salt = salt,
            Role = role
        };

        await _userRepository.AddAsync(user);

        return (true, null);
    }


    public User? GetCurrentUser() => _currentUser;






    private string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        return Convert.ToBase64String(saltBytes);
    }



    private string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {

            byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);

            byte[] hashBytes = sha256.ComputeHash(saltedPassword);

            return Convert.ToBase64String(hashBytes);
        }

    }




}
