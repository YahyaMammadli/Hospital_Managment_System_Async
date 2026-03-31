using HospitalManagementSystem.Domain.Entities;

namespace HospitalManagementSystem.Application.Interfaces.Repositories;

public interface IDoctorRepository
{
    Task<IEnumerable<Doctor>> GetAllAsync();
    Task<Doctor?> GetByIdAsync(int id);
    Task AddAsync(Doctor doctor);
}