using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Persistence.Data;
using HospitalManagementSystem.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetByIdAsync(int id)
      => await _context
        .Patients
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == id);

    
    
    public async Task<IEnumerable<Patient>> GetAllActiveAsync()
       => await _context
        .Patients
        .AsNoTracking()
        .Where(p => p.IsActive)
        .ToListAsync();




    public async Task<IEnumerable<Patient>> SearchByDiagnosisAsync(string diagnosis) 
        => await _context.Patients
            .AsNoTracking()
            .Where(p => p.Diagnosis
            .ToLower()
            .Contains(diagnosis.ToLower()))
            .ToListAsync();

    
    
    public async Task AddAsync(Patient patient)
    {
        
        await _context.Patients.AddAsync(patient);
    
        await _context.SaveChangesAsync();
    
    }

    
    
    public async Task UpdateStatusAsync(int patientId, bool isActive)
    {
        
        var patient = await _context.Patients.FindAsync(patientId);
        
        if (patient != null)
        {
        
            patient.IsActive = isActive;
            
            await _context.SaveChangesAsync();
        
        }

    }


    public async Task<IEnumerable<Patient>> GetPatientsByDoctorIdAsync(int doctorId)
    {
        return await _context.Patients
            .Where(p => _context.Appointments.Any(a => a.PatientId == p.Id && a.DoctorId == doctorId))
            .AsNoTracking()
            .ToListAsync();
    }



}
