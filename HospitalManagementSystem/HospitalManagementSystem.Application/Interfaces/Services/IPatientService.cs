using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.DTOs;

namespace HospitalManagementSystem.Application.Interfaces.Services;

public interface IPatientService
{
    Task<PatientSummaryDto> GetPatientSummaryAsync(int patientId, User? currentUser);
    Task ProcessPatientsAsync(IEnumerable<int> patientIds);
    Task<Patient?> GetByIdAsync(int id);
    Task<IEnumerable<Patient>> GetAllActiveAsync(User? currentUser = null);
    Task<IEnumerable<Patient>> SearchByDiagnosisAsync(string diagnosis);
    Task AddAsync(Patient patient);
    Task UpdateStatusAsync(int patientId, bool isActive);
}