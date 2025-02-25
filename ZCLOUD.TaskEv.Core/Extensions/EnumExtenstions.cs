
using ZCLOUD.TaskEv.Data.Enums;

namespace ZCLOUD.TaskEv.Core.Extensions;

public static class EnumExtenstions
{
    public static string GetPriorityColor(this Priority priority) => priority switch
    {
        Priority.Low => "secondary",
        Priority.Medium => "info",
        Priority.High => "warning",
        Priority.Critical => "danger",
        _ => "secondary"
    };

    public static string GetStatusColor(this EvTaskStatus status) => status switch
    {
        EvTaskStatus.New => "primary",
        EvTaskStatus.InProgress => "info",
        EvTaskStatus.Blocked => "warning",
        EvTaskStatus.Resolved => "success",
        EvTaskStatus.Closed => "secondary",
        _ => "secondary"
    };

}
