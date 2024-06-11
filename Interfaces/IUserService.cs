using ThoughtKeeper;

public interface IUserService
{
    bool DoesUsernameExist(string username);
    void CreateUser(string username, string password);
    UserDTO GetUserById(int userId);
    UserDTO GetUserByUsername(string username);
}