
namespace ZCLOUD.TaskEv.Data.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string SenderName { get; set; }
    public string Message { get; set; }
    public DateTime SendDate { get; set; }

    public EvTask Task { get; set; }
}
