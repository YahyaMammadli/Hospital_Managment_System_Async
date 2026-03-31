using HospitalManagementSystem.Application.Interfaces.Repositories;
using HospitalManagementSystem.Persistence.Data;


namespace HospitalManagementSystem.Infrastructure.Repositories;
public class PatientRepositoryFactory : IPatientRepositoryFactory
{
    public IPatientRepository Create()
    {
        var context = new AppDbContext();

        return new PatientRepository(context);
    
    }

}

