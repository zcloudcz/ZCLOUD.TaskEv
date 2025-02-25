using Microsoft.AspNetCore.Components;


namespace ZCLOUD.TaskEv.Components.Pages;
public partial class TaskDetail
{
    [Parameter]
    public int TaskId { get; set; }

    [Parameter]
    public EventCallback OnCloseTask { get; set; }

    [Parameter]
    public EventCallback OnSaveNew { get; set; }

    public void OnClose()
    {
        if (OnCloseTask.HasDelegate)
            OnCloseTask.InvokeAsync();
        else
            NavigationManager.NavigateTo($"/");
    }

    private EvTask? task = new();
    private EvTask editTask = new();
    private bool isEditing;

    private TaskChat chatComponent;
    private TaskDocuments documentsComponent;
    private TaskChecklist checklistComponent;

    protected override async Task OnInitializedAsync()
    {
        await LoadTask();
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        await LoadTask();
    }

    private async Task LoadTask()
    {
        using var context = await ContextFactory.CreateDbContextAsync();

        task = await context.EvTasks
            .Include(t => t.Communications)
            .Include(t => t.Documents)
            .Include(t => t.ChecklistItems)
            .FirstOrDefaultAsync(t => t.Id == TaskId);

        if (task == null)
        {
            task = new();
            editTask = new()
            {
                ReportedDate = DateTime.Now,
                Deadline = DateTime.Now.AddDays(7),
                Status = EvTaskStatus.New,
                AssignedBy = UserService.CurrentUser,
                AssignedTo = string.Empty,
                Description = string.Empty,
                CompanyName = string.Empty,
                ChecklistItems = new(),
                Communications = new(),
                Documents = new()

            };
        }
        else
            editTask = new EvTask
            {
                Id = task.Id,
                CompanyName = task.CompanyName,
                Description = task.Description,
                AssignedBy = task.AssignedBy,
                AssignedTo = task.AssignedTo,
                ReportedDate = task.ReportedDate,
                Deadline = task.Deadline,
                Priority = task.Priority,
                Status = task.Status
            };
    }

    private void StartEdit()
    {
        isEditing = true;
    }

    private void CancelEdit()
    {
        editTask = new EvTask
        {
            Id = task!.Id,
            CompanyName = task.CompanyName,
            Description = task.Description,
            AssignedBy = task.AssignedBy,
            AssignedTo = task.AssignedTo,
            ReportedDate = task.ReportedDate,
            Deadline = task.Deadline,
            Priority = task.Priority,
            Status = task.Status
        };
        isEditing = false;
    }

    private async Task HandleValidSubmit()
    {
        task.CompanyName = editTask.CompanyName;
        task.Description = editTask.Description;
        task.AssignedBy = editTask.AssignedBy;
        task.AssignedTo = editTask.AssignedTo;
        task.Deadline = editTask.Deadline;
        task.Priority = editTask.Priority;
        task.Status = editTask.Status;

        using var context = await ContextFactory.CreateDbContextAsync();

        if (TaskId > 0)
            context.EvTasks.Update(task);
        else
            context.EvTasks.Add(task);

        await context.SaveChangesAsync();

        if (TaskId == 0)
        {
            if (OnSaveNew.HasDelegate)
                await OnSaveNew.InvokeAsync();
        }

        isEditing = false;
        TaskId = task.Id;


        UserService.AddUsers(task.AssignedBy, task.AssignedTo);
    }
}
