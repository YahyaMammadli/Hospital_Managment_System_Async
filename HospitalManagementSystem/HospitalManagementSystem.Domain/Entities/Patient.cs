using HospitalManagementSystem.Domain.Enums;

namespace HospitalManagementSystem.Domain.Entities;




public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;    
    public string Surname { get; set; } = string.Empty;     
    public DateTime DateOfBirth { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    
    public string FullName => $"{Name} {Surname}";


    public ICollection<Appointment> Appointments { get; set; } = Enumerable.Empty<Appointment>().ToList();

    public override string ToString()
    {
        return $"$\"ID: {Id}, Name: {FullName}, Diagnosis: {Diagnosis}, Active: {IsActive}\n";
    }


}
