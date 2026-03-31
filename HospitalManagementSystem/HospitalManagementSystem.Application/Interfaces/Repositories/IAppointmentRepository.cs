using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.Application.Interfaces.Repositories;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);
    Task<bool> DoctorHasAppointmentAsync(int doctorId, DateTime at);
    Task AddAsync(Appointment appointment);
    Task UpdateStatusAsync(int appointmentId, AppointmentStatus status);
}