using Microsoft.AspNetCore.Components;

namespace ZCLOUD.TaskEv.Components.Pages;
public partial class TaskTable
{
    [Parameter]
    public List<EvTask> Tasks { get; set; } = new();

    [Parameter]
    public bool ShowChecklistStatus { get; set; }

    [Parameter]
    public EventCallback<int> OnTaskSelected { get; set; }

    private string GetRowClass(EvTask task)
    {
        if (task.Status == EvTaskStatus.Resolved || task.Status == EvTaskStatus.Closed)
            return "";

        if (task.Deadline < DateTime.Now)
            return "table-danger";

        if (ShowChecklistStatus &&
            task.ChecklistItems?.Any(i => !i.IsCompleted && i.Deadline < DateTime.Now) == true)
            return "table-warning";

        return "";
    }
}
