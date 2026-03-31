using HospitalManagementSystem.Domain.Entities;

namespace HospitalManagementSystem.Application.Interfaces.Services;

public interface IAppointmentService
{
    Task<Appointment> ScheduleAppointmentAsync(int patientId, int? doctorId, DateTime at, User? currentUser);
    Task TransferPatientAsync(int patientId, int fromDoctorId, int toDoctorId, User? currentUser);
}