public interface IAuthenticationService
{
    bool Login(string username, string password);
    bool VerifyPassword(string enteredPassword, string storedHash);
}