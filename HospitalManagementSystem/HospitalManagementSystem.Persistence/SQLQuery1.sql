
GO
INSERT INTO Patients (Name, Surname, DateOfBirth, Diagnosis, IsActive)
VALUES
('John', 'Smith', '1985-03-12', 'Flu', 1),
('Emily', 'Johnson', '1990-07-25', 'Migraine', 1),
('Michael', 'Brown', '1978-11-02', 'Hypertension', 1),
('Sarah', 'Davis', '2000-01-15', 'Asthma', 1),
('Daniel', 'Wilson', '1965-09-30', 'Diabetes', 0);


GO
INSERT INTO Doctors (Name, Surname, Specialization, RoomNumber)
VALUES
('Alice', 'Taylor', 'Therapist', '101'),
('Robert', 'Anderson', 'Neurologist', '202'),
('David', 'Thomas', 'Cardiologist', '303');


GO
INSERT INTO Appointments (PatientId, DoctorId, ScheduledAt, Status)
VALUES
(1, 1, '2026-03-25 10:00:00', 'Scheduled'),
(2, 2, '2026-03-25 11:00:00', 'Cancelled'),
(3, 3, '2026-03-26 09:30:00', 'Completed'),
(4, 1, '2026-03-26 14:00:00', 'Completed'),
(5, 3, '2026-03-27 16:00:00', 'Completed');


TRUNCATE TABLE Appointments



