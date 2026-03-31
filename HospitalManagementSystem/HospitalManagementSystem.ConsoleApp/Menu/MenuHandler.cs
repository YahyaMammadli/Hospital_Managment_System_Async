using HospitalManagementSystem.Application.Interfaces.Repositories;
using HospitalManagementSystem.Application.Interfaces.Services;
using HospitalManagementSystem.Domain.Entities;
using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.ConsoleApp.Menu;

public class MenuHandler
{
    private readonly IAuthService _authService;
    private readonly IPatientService _patientService;
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorRepository _doctorRepository;

    private User? _currentUser;

    public MenuHandler(IAuthService authService,IPatientService patientService,IAppointmentService appointmentService,IDoctorRepository doctorRepository)
    {
        _authService = authService;
        _patientService = patientService;
        _appointmentService = appointmentService;
        _doctorRepository = doctorRepository;
    }

    


    public async Task RunAsync()
    {
        while (_currentUser == null)
        {
            Console.Clear();
            Console.Write("\n\n===== Hospital Management System =====\n");
            Console.Write("1. Register\n");
            Console.Write("2. Login\n");
            Console.Write("3. Exit\n");
            Console.Write("Select option => ");
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await RegisterAsync();
                        break;
                    case "2":
                        await LoginAsync();
                        break;
                    case "3":
                        return;
                    default:
                        Console.Write("Invalid option!\n");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Write($"Error: {ex.Message}\n");
            }

            if (_currentUser == null)
            {
                Console.Write("Press any key to continue...\n");
                Console.ReadKey();
            }
        }

        while (true)
        {
            Console.Clear();
            Console.Write("\n\n===== Hospital Management System =====\n");
            Console.Write($"Logged in as: {_currentUser.Username} ({_currentUser.Role})\n\n");

            int optionNumber = 1;
            Console.Write($"{optionNumber++}. Show Patients\n");
            Console.Write($"{optionNumber++}. Schedule Appointment\n");
            Console.Write($"{optionNumber++}. Transfer Patient\n");
            Console.Write($"{optionNumber++}. Process Patients\n");
            Console.Write($"{optionNumber++}. User Summary Data\n");   

            int logoutOption = optionNumber++;
            Console.Write($"{logoutOption}. Logout\n");
            int exitOption = optionNumber++;
            Console.Write($"{exitOption}. Exit\n");

            Console.Write("Select option => ");
            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1": await ShowPatientsAsync(); break;
                    case "2": await ScheduleAsync(); break;
                    case "3": await TransferAsync(); break;
                    case "4": await ProcessAsync(); break;
                    case "5": await ShowPatientSummaryAsync(); break;
                    case string s when s == logoutOption.ToString():
                        _currentUser = null;
                        await RunAsync();
                        return;
                    case string s when s == exitOption.ToString():
                        return;
                    default:
                        Console.Write("Invalid option!\n");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Write($"Error: {ex.Message}\n");
            }

            Console.Write("Press any key to continue...\n");
            Console.ReadKey();
        }
    }



    private async Task LoginAsync()
    {
        Console.Write("Username => ");
        var username = Console.ReadLine();
        Console.Write("Password => ");
        var password = ReadPassword();

        var user = await _authService.LoginAsync(username, password);
        if (user == null)
        {
            Console.Write("Login failed!\n");
            return;
        }

        _currentUser = user;
        Console.Write($"Welcome, {user.Username}!\n");
    }











    private async Task RegisterAsync()
    {
        Console.Write("Username => ");
        var username = Console.ReadLine();
        Console.Write("Password => ");
        var password = Console.ReadLine();

        Console.Write("Role (Admin/Doctor) => ");

        var roleInput = Console.ReadLine();

        if (!Enum.TryParse<UserRole>(roleInput, true, out var role))
        {
            Console.Write("Invalid role. Use Admin or Doctor!\n");
            return;
        }

        
        
            var res = await _authService.RegisterAsync(username, password, role);
            if (res == (true, null))
            {
                Console.Write("Registration successful!\n");
                return;
            }


            Console.Write("Registration failed!\n");
        
    }





    private async Task ShowPatientSummaryAsync()
    {
        if (!EnsureLoggedIn()) return;

        Console.Write("Patient ID: ");
        if (!int.TryParse(Console.ReadLine(), out var patientId))
        {
            Console.Write("Invalid patient ID.\n");
            return;
        }

        try
        {
            var summary = await _patientService.GetPatientSummaryAsync(patientId, _currentUser);
            Console.Write($"\nPatient: {summary.Patient.FullName}\n");
            Console.Write($"Total appointments: {summary.TotalAppointments}\n");
            Console.Write($"Completed appointments: {summary.CompletedAppointments}\n");
        }
        catch (Exception ex)
        {
            Console.Write($"Error retrieving summary: {ex.Message}\n");
        }
    }






    private async Task ShowPatientsAsync()
    {
        if (!EnsureLoggedIn()) return;

        Console.Write("Search by diagnosis (or press Enter to list all active) => ");
        var diagnosis = Console.ReadLine();

        IEnumerable<Patient> patients;
        if (string.IsNullOrWhiteSpace(diagnosis))
        {
            patients = await _patientService.GetAllActiveAsync(_currentUser);
        }
        else
        {
            patients = await _patientService.SearchByDiagnosisAsync(diagnosis);
        }

        if (_currentUser.Role == UserRole.Doctor && _currentUser.DoctorId.HasValue)
        {
            Console.Write("Showing only patients assigned to you.\n\n");
        }

        Console.Write("Patients:\n");
        foreach (var p in patients)
        {
            Console.Write($"{p.Id}: {p.FullName} (Diagnosis: {p.Diagnosis}, Active: {p.IsActive})\n");
        }
    }


    private async Task ScheduleAsync()
    {
        if (!EnsureLoggedIn()) return;

        int? doctorId = null;
        if (_currentUser.Role == UserRole.Admin)
        {
            var doctors = await _doctorRepository.GetAllAsync();
            if (!doctors.Any())
            {
                Console.Write("No doctors available.\n");
                return;
            }
            Console.Write("Available doctors:\n");
            foreach (var d in doctors)
            {
                Console.Write($"ID: {d.Id}, Name: {d.FullName}, Specialization: {d.Specialization}\n");
            }
            Console.Write("Doctor ID: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.Write("Invalid doctor ID.\n");
                return;
            }
            doctorId = id;
        }

        Console.Write("Patient ID: ");
        if (!int.TryParse(Console.ReadLine(), out var patientId))
        {
            Console.Write("Invalid patient ID.\n");
            return;
        }

        Console.Write("Date and time (yyyy-mm-dd HH:MM): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var dateTime))
        {
            Console.Write("Invalid date/time.\n");
            return;
        }

        try
        {
            var appointment = await _appointmentService.ScheduleAppointmentAsync(patientId, doctorId, dateTime, _currentUser);
            Console.Write($"Appointment scheduled with ID: {appointment.Id}\n");
        }
        catch (InvalidOperationException ex)
        {
            Console.Write($"Cannot schedule: {ex.Message}\n");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.Write($"Access denied: {ex.Message}\n");
        }
    }

    


    private async Task TransferAsync()
    {
        if (!EnsureLoggedIn()) return;

        if (_currentUser.Role != UserRole.Admin)
        {
            Console.Write("Only Admin can transfer patients.\n");
            return;
        }

        Console.Write("Patient ID: ");
        if (!int.TryParse(Console.ReadLine(), out var patientId)) return;
        Console.Write("From Doctor ID: ");
        if (!int.TryParse(Console.ReadLine(), out var fromDoctorId)) return;
        Console.Write("To Doctor ID: ");
        if (!int.TryParse(Console.ReadLine(), out var toDoctorId)) return;

        try
        {
            await _appointmentService.TransferPatientAsync(patientId, fromDoctorId, toDoctorId, _currentUser);
            Console.Write("Transfer successful.\n");
        }
        catch (InvalidOperationException ex)
        {
            Console.Write($"Transfer failed: {ex.Message}\n");
        }
    }




    private async Task ProcessAsync()
    {
        if (!EnsureLoggedIn()) return;

        Console.Write("Enter patient IDs separated by commas: ");
        var input = Console.ReadLine();
        var ids = input?.Split(',').Select(s => int.TryParse(s.Trim(), out var id) ? id : (int?)null)
                    .Where(id => id.HasValue).Select(id => id.Value).ToList();

        if (ids == null || !ids.Any())
        {
            Console.Write("No valid IDs provided.\n");
            return;
        }

        await _patientService.ProcessPatientsAsync(ids);
        Console.Write("Processing completed.\n");
    }

    
    
    
    private bool EnsureLoggedIn()
    {
        if (_currentUser == null)
        {
            Console.Write("You must be logged in to perform this action.\n");
            return false;
        }
        return true;
    }

    
    
    private string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Remove(password.Length - 1);
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.Write("\n");
        return password;
    }



}
