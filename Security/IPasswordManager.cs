namespace ThoughtKeeper.Security
{
    public interface IPasswordManager
    {
        string HashPassword(string password);
        bool VerifyPassword(string username, string password);
        bool IsPasswordValid(string password);
    }
}
