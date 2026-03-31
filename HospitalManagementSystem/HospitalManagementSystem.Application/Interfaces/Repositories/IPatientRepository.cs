using HospitalManagementSystem.Domain.Entities;

namespace HospitalManagementSystem.Application.Interfaces.Repositories;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(int id);
    Task<IEnumerable<Patient>> GetAllActiveAsync();
    Task<IEnumerable<Patient>> SearchByDiagnosisAsync(string diagnosis);
    Task<IEnumerable<Patient>> GetPatientsByDoctorIdAsync(int doctorId);
    Task AddAsync(Patient patient);
    Task UpdateStatusAsync(int patientId, bool isActive);
}