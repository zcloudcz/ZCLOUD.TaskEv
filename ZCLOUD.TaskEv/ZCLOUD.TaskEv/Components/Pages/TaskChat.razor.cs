using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;

namespace ZCLOUD.TaskEv.Components.Pages;
public partial class TaskChat
{
    [Parameter]
    public int TaskId { get; set; }

    private List<ChatMessage> messages = new();
    private string newMessage = "";

    protected override async Task OnInitializedAsync()
    {
        await LoadMessages();
        UserService.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        UserService.OnChange -= StateHasChanged;
    }

    private async Task LoadMessages()
    {
        using var context = await ContextFactory.CreateDbContextAsync();

        messages = await context.ChatMessages
            .Where(m => m.TaskId == TaskId)
            .OrderBy(m => m.SendDate)
            .ToListAsync();
    }

    private async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(newMessage)) return;

        using var context = await ContextFactory.CreateDbContextAsync();

        var message = new ChatMessage
        {
            TaskId = TaskId,
            SenderName = UserService.CurrentUser,
            Message = newMessage,
            SendDate = DateTime.Now
        };

        context.ChatMessages.Add(message);
        await context.SaveChangesAsync();
        messages.Add(message);
        newMessage = "";
    }

    private async Task HandleKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await SendMessage();
        }
    }

    private bool IsCurrentUser(string senderName) => senderName == UserService.CurrentUser;
}
