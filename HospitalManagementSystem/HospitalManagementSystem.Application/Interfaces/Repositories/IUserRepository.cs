using HospitalManagementSystem.Domain.Entities;

namespace HospitalManagementSystem.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}
