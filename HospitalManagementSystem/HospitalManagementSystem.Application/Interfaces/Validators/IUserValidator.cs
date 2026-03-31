

namespace HospitalManagementSystem.Application.Interfaces.Validators
{
    public interface IUserValidator
    {
        public bool ValidateUsername(string username);

        public bool ValidatePassword(string password);


    }
}
