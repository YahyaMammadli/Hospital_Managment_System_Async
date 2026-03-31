using HospitalManagementSystem.Application.Interfaces.Repositories;
using HospitalManagementSystem.Application.Interfaces.Services;
using HospitalManagementSystem.Domain.DTOs;
using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepositoryFactory _patientRepositoryFactory;

    public PatientService(IPatientRepository patientRepository, IAppointmentRepository appointmentRepository, IPatientRepositoryFactory patientRepositoryFactory)
    {
        _patientRepository = patientRepository;
        _appointmentRepository = appointmentRepository;
        _patientRepositoryFactory = patientRepositoryFactory;
    }




    public async Task<PatientSummaryDto> GetPatientSummaryAsync(int patientId, User? currentUser)
    {
        var patient = await _patientRepository.GetByIdAsync(patientId);

        if (patient == null)
            throw new InvalidOperationException($"Patient with id {patientId} not found.");

        if (currentUser?.Role == UserRole.Doctor && currentUser.DoctorId.HasValue)
        {
            var doctorPatients = await _patientRepository.GetPatientsByDoctorIdAsync(currentUser.DoctorId.Value);
            if (!doctorPatients.Any(p => p.Id == patientId))
                throw new UnauthorizedAccessException("You are not allowed to view this patient's summary.");
        
        }

        var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);

        return new PatientSummaryDto
        {
            Patient = patient,
            TotalAppointments = appointments.Count(),
            CompletedAppointments = appointments.Count(a => a.Status == AppointmentStatus.Completed)
        };

    }



    

    public async Task ProcessPatientsAsync(IEnumerable<int> patientIds)
    {

        int maxConcurrency = 3;
        using var semaphore = new SemaphoreSlim(maxConcurrency);

        var tasks = patientIds.Select(async patientId =>
        {
            await semaphore.WaitAsync();

            try
            {
                var patientRepository = _patientRepositoryFactory.Create();

                var patient = await patientRepository.GetByIdAsync(patientId);
                
                if (patient == null)
                {
                    Console.WriteLine($"Patient {patientId} not found.");
                    return;
                }

                await Task.Delay(Random.Shared.Next(200, 500));

                Console.WriteLine($"Processed patient: {patient.FullName} (ID: {patient.Id})");
            
            }

            finally
            {
                semaphore.Release();
            }


        });

    
        await Task.WhenAll(tasks);
    
    
    
    }




    public async Task<IEnumerable<Patient>> GetAllActiveAsync(User? currentUser = null)
    {
        if (currentUser?.Role == UserRole.Doctor && currentUser.DoctorId.HasValue)
            return await _patientRepository
                         .GetPatientsByDoctorIdAsync(currentUser.DoctorId.Value);
        

        return await _patientRepository.GetAllActiveAsync();
    
    
    }



    public async Task<Patient?> GetByIdAsync(int id) => await _patientRepository.GetByIdAsync(id);
    
    public async Task<IEnumerable<Patient>> SearchByDiagnosisAsync(string diagnosis) => await _patientRepository.SearchByDiagnosisAsync(diagnosis);
    
    public async Task AddAsync(Patient patient) => await _patientRepository.AddAsync(patient);
    
    public async Task UpdateStatusAsync(int patientId, bool isActive) => await _patientRepository.UpdateStatusAsync(patientId, isActive);


}
