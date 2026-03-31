

using HospitalManagementSystem.Application.Interfaces.Validators;

namespace HospitalManagementSystem.Application.Validators;

public class UserValidator : IUserValidator
{

    public bool ValidateUsername(string username)
    {

        if (string.IsNullOrWhiteSpace(username)) 
            return false;

        if (username.Length < 3)
            return false;


        return true;

    }

    public bool ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;
        //8
        if (password.Length < 3 )
            return false;

        if (!password.Any(char.IsUpper))
            return false;

        if (!password.Any(char.IsDigit))
            return false;

        return true;

    }



}
