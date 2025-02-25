

namespace ZCLOUD.TaskEv.Core.Interfaces;

public interface IUserService
{
    event Action? OnChange;

    List<string> GetAllUsers();
    void AddUsers(params string[] usernames);

    public string CurrentUser { get; set; }

}
