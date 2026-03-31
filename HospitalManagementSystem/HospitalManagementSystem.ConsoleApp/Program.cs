using HospitalManagementSystem.Application.Interfaces.Repositories;
using HospitalManagementSystem.Application.Interfaces.Services;
using HospitalManagementSystem.Application.Interfaces.Validators;
using HospitalManagementSystem.Application.Services;
using HospitalManagementSystem.Application.Validators;
using HospitalManagementSystem.ConsoleApp.Menu;
using HospitalManagementSystem.Infrastructure.Repositories;
using HospitalManagementSystem.Persistence.Data;

namespace HospitalManagementSystem.ConsoleApp;

public class Program
{
    static async Task Main()
    {
        using var dbContext = new AppDbContext();


        IPatientRepository patientRepository = new PatientRepository(dbContext);
        IAppointmentRepository appointmentRepository = new AppointmentRepository(dbContext);
        IUserRepository userRepository = new UserRepository(dbContext);
        IDoctorRepository doctorRepository = new DoctorRepository(dbContext);

        IUserValidator userValidator = new UserValidator();

        IPatientRepositoryFactory patientRepositoryFactory = new PatientRepositoryFactory();

        IAuthService authService = new AuthService(userRepository, userValidator);
        IPatientService patientService = new PatientService(patientRepository, appointmentRepository, patientRepositoryFactory);
        IAppointmentService appointmentService = new AppointmentService(appointmentRepository, patientRepository);

        var menuHandler = new MenuHandler(authService, patientService, appointmentService, doctorRepository);

        await menuHandler.RunAsync();


    }
}
