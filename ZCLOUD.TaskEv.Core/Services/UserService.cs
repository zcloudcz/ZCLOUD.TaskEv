using ZCLOUD.TaskEv.Core.Interfaces;

namespace ZCLOUD.TaskEv.Core.Services;

public class UserService : IUserService
{
    private string _currentUser = "Jan Novák"; // Výchozí uživatel
    private List<string> _users = new List<string>
    {
        "Jan Novák",
        "Petr Svoboda",
        "Marie Dvořáková",
        "Pavel Černý",
        "Lucie Veselá"
    };

    public void AddUsers(params string[] usernames)
    {
        foreach(var username in usernames)
            if (!_users.Contains(username))
                _users.Add(username);

        _users.Sort();

        var u = CurrentUser;
        CurrentUser = u;

    }

    public event Action? OnChange;

    public string CurrentUser
    {
        get => _currentUser;
        set
        {
            if (_currentUser != value)
            {
                _currentUser = value;
                NotifyStateChanged();
            }
        }
    }

    public List<string> GetAllUsers() => _users;

    private void NotifyStateChanged() => OnChange?.Invoke();
}