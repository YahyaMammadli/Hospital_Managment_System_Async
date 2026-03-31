

using HospitalManagementSystem.Application.Interfaces.Repositories;
using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;
using HospitalManagementSystem.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }



    public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId) 
        => await _context
                .Appointments
                .AsNoTracking()
                .Where(a => a.PatientId == patientId)
                .ToListAsync();


    
    public async Task<bool> DoctorHasAppointmentAsync(int doctorId, DateTime at) 
        => await _context
                .Appointments
                .AnyAsync(a => a.DoctorId == doctorId &&
                            a.ScheduledAt.Date == at.Date &&
                            a.Status != AppointmentStatus.Cancelled);



    public async Task AddAsync(Appointment appointment)
    {

        await _context.Appointments.AddAsync(appointment);
        
        await _context.SaveChangesAsync();
    
    
    }




    public async Task UpdateStatusAsync(int appointmentId, AppointmentStatus status)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        
        if (appointment != null)
        {
            appointment.Status = status;
            await _context.SaveChangesAsync();
        
        }
    
    
    }



}
