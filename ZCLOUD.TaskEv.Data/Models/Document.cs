
namespace ZCLOUD.TaskEv.Data.Models;

public class Document
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadDate { get; set; }

    public EvTask Task { get; set; }
}
