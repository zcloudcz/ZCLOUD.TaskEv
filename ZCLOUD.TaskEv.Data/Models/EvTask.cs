using ZCLOUD.TaskEv.Data.Enums;

namespace ZCLOUD.TaskEv.Data.Models;

public class EvTask
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string Description { get; set; }
    public string AssignedBy { get; set; }
    public string AssignedTo { get; set; }
    public DateTime ReportedDate { get; set; }
    public DateTime? ResolvedDate { get; set; }
    public DateTime Deadline { get; set; }
    public Priority Priority { get; set; }
    public EvTaskStatus Status { get; set; }

    // Navigační vlastnosti
    public List<ChatMessage> Communications { get; set; }
    public List<Document> Documents { get; set; }
    public List<ChecklistItem> ChecklistItems { get; set; }
}
