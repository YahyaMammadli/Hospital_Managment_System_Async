namespace HospitalManagementSystem.Domain.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int RoomNumber { get; set; }

        //Вычисляемое поле
        public string FullName => $"{Name} {Surname}";


        public ICollection<Appointment> Appointments { get; set; } = Enumerable.Empty<Appointment>().ToList();
        public User? User { get; set; }
        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name} {Surname}, Specialization: {Specialization}\n\n";
        }


    }

}
