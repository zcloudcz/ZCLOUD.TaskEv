using ZCLOUD.TaskEv.Core.Enums;

namespace ZCLOUD.TaskEv.Components.Pages;
public partial class Tasks
{
    private IQueryable<EvTask> allTasks;
    private List<EvTask> filteredTasks = new();
    private TasksView currentView = TasksView.All;
    private int? selectedTaskId;

    bool ShowChecklists = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadTasksAsync();
    }

    private async Task LoadTasksAsync()
    {
        using var context = await ContextFactory.CreateDbContextAsync();

        allTasks = context.EvTasks
            .Include(t => t.ChecklistItems);

        await FilterTasksAsync();
    }

    private async Task FilterTasksAsync()
    {
        var q = allTasks.Where(t => t.Status != EvTaskStatus.Resolved && t.Status != EvTaskStatus.Closed);

        filteredTasks = currentView switch
        {
            TasksView.Unresolved => await q.ToListAsync(),
            TasksView.Overdue => await q.Where(t => t.Deadline < DateTime.Now).ToListAsync(),
            TasksView.Assignee => await allTasks.Where(t => t.AssignedTo == UserService.CurrentUser).ToListAsync(),
            TasksView.Assigner => await allTasks.Where(t => t.AssignedBy == UserService.CurrentUser).ToListAsync(),
            TasksView.ChecklistOverdue => await q.Where(t => t.ChecklistItems.Any(ci => !ci.IsCompleted && ci.Deadline < DateTime.Now)).ToListAsync(),
            _ => await allTasks.ToListAsync()
        };
    }

    private async Task ChangeView(TasksView view)
    {
        currentView = view;
        ShowChecklists = (currentView == TasksView.ChecklistOverdue);
        await LoadTasksAsync();
    }

    private bool IsOverdue(EvTask task)
    {
        return task.Deadline < DateTime.Now && task.Status != EvTaskStatus.Resolved && task.Status != EvTaskStatus.Closed;
    }


    private void OpenTaskDetail(int taskId)
    {
        selectedTaskId = taskId;
    }

    private Task CloseTaskDetail()
    {
        selectedTaskId = null;
        return LoadTasksAsync();
    }

    private void CreateNewTask()
    {
        selectedTaskId = 0;
    }
}
