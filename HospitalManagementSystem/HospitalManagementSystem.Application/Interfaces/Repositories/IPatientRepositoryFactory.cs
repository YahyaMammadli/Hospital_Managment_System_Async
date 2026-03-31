using HospitalManagementSystem.Domain.Entities;



namespace HospitalManagementSystem.Application.Interfaces.Repositories;

public interface IPatientRepositoryFactory
{

    IPatientRepository Create();

}
