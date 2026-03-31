

using HospitalManagementSystem.Domain.Entities;

namespace HospitalManagementSystem.Domain.DTOs;

public class PatientSummaryDto
{

    public Patient Patient { get; set; } = null!;
    public int TotalAppointments { get; set; }
    public int CompletedAppointments { get; set; }



    public override string ToString()
    {
        return $"\n\nPatient => {Patient}\nTotal Appointments => {TotalAppointments}\nCompleted Appointments => {CompletedAppointments}\n\n";
    }

}
