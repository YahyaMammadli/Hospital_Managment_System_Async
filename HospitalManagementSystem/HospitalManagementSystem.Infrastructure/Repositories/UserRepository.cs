using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Application.Interfaces.Repositories;



namespace HospitalManagementSystem.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {

        await _context.Users.AddAsync(user);
        
        await _context.SaveChangesAsync();
    
    }


    public async Task<User?> GetByUsernameAsync(string username) 
        => await _context
                    .Users
                    .AsNoTracking()
                    .Include(u => u.Doctor)
                    .FirstOrDefaultAsync(u => u.Username == username);


}
