using HospitalManagementSystem.Application.Interfaces.Repositories;
using HospitalManagementSystem.Application.Interfaces.Services;
using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;



    public AppointmentService(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
    }




    public async Task<Appointment> ScheduleAppointmentAsync(int patientId, int? doctorId, DateTime at, User? currentUser)
    {
        var patient = await _patientRepository.GetByIdAsync(patientId);
        
        if (patient == null)
            throw new InvalidOperationException("Patient not found.");

        int finalDoctorId;
        
        if (currentUser?.Role == UserRole.Doctor)
        {
            if (!currentUser.DoctorId.HasValue)
                throw new InvalidOperationException("Doctor user is not linked to a doctor record.");
        
            finalDoctorId = currentUser.DoctorId.Value;
        }


        else if (currentUser?.Role == UserRole.Admin)
        {
            if (!doctorId.HasValue)
                throw new ArgumentException("Admin must specify a doctor ID.");
            
            
            finalDoctorId = doctorId.Value;
        }


        else
        {
            throw new UnauthorizedAccessException("Only Admin or Doctor can schedule appointments.");
        }

        bool isDoctorBusy = await _appointmentRepository.DoctorHasAppointmentAsync(finalDoctorId, at);
        
        if (isDoctorBusy)
            throw new InvalidOperationException("Doctor is already booked at that time.");


        var appointment = new Appointment
        {
            PatientId = patientId,
            DoctorId = finalDoctorId,
            ScheduledAt = at,
            Status = AppointmentStatus.Scheduled
        };

        await _appointmentRepository.AddAsync(appointment);
        
        
        return appointment;
    
    }




    public async Task TransferPatientAsync(int patientId, int fromDoctorId, int toDoctorId, User? currentUser)
    {
        if (currentUser?.Role != UserRole.Admin)
            throw new UnauthorizedAccessException("Only Admin can transfer patients.");

        var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);
        
        var activeAppointment = appointments.FirstOrDefault(a => a.DoctorId == fromDoctorId && a.Status == AppointmentStatus.Scheduled);

        if (activeAppointment == null)
            throw new InvalidOperationException("No active appointment found with the current doctor.");

        await _appointmentRepository.UpdateStatusAsync(activeAppointment.Id, AppointmentStatus.Cancelled);
       
        await ScheduleAppointmentAsync(patientId, toDoctorId, activeAppointment.ScheduledAt, currentUser);
    }
}