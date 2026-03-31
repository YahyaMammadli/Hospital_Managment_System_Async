

using HospitalManagementSystem.Application.Interfaces.Repositories;
using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly AppDbContext _context;

    public DoctorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync()
    {
        return await _context.Doctors.AsNoTracking().ToListAsync();
    }




    public async Task<Doctor?> GetByIdAsync(int id)
    {
        return await _context.Doctors.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
    }




    public async Task AddAsync(Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();
    }


}
