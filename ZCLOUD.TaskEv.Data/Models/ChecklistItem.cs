
namespace ZCLOUD.TaskEv.Data.Models;

public class ChecklistItem
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? CompletedDate { get; set; }

    public EvTask Task { get; set; }
}
