using Microsoft.AspNetCore.Components;

namespace ZCLOUD.TaskEv.Components.Pages;
public partial class TaskChecklist
{
    [Parameter]
    public int TaskId { get; set; }

    private List<ChecklistItem> checklistItems = new();
    private string newItemDescription = "";
    private DateTime? newItemDeadline;

    protected override async Task OnInitializedAsync()
    {
        await LoadChecklistItems();
    }

    private async Task LoadChecklistItems()
    {
        using var context = await ContextFactory.CreateDbContextAsync();

        checklistItems = await context.ChecklistItems
            .Where(c => c.TaskId == TaskId)
            .ToListAsync();
    }

    private async Task AddChecklistItem()
    {
        if (string.IsNullOrWhiteSpace(newItemDescription)) return;

        var item = new ChecklistItem
        {
            TaskId = TaskId,
            Description = newItemDescription,
            Deadline = newItemDeadline,
            IsCompleted = false
        };

        using var context = await ContextFactory.CreateDbContextAsync();

        context.ChecklistItems.Add(item);
        await context.SaveChangesAsync();
        checklistItems.Add(item);

        newItemDescription = "";
        newItemDeadline = null;
    }

    private async Task ToggleItemCompletion(ChecklistItem item)
    {
        item.IsCompleted = !item.IsCompleted;
        item.CompletedDate = item.IsCompleted ? DateTime.Now : null;

        using var context = await ContextFactory.CreateDbContextAsync();

        context.ChecklistItems.Update(item);
        await context.SaveChangesAsync();
    }

    private async Task DeleteChecklistItem(ChecklistItem item)
    {
        using var context = await ContextFactory.CreateDbContextAsync();

        context.ChecklistItems.Remove(item);
        await context.SaveChangesAsync();
        checklistItems.Remove(item);
    }

    private bool IsOverdue(ChecklistItem item)
    {
        return !item.IsCompleted && item.Deadline.HasValue && item.Deadline.Value < DateTime.Now;
    }
}
