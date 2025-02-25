
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZCLOUD.TaskEv.Data.Enums;
using ZCLOUD.TaskEv.Data.Models;

namespace ZCLOUD.TaskEv.Data.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, bool useInMemoryDb)
    {
        using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        if (!useInMemoryDb)
        {
            context.Database.Migrate();
        }

        if (context.EvTasks.Any())
        {
            return;   // DB již obsahuje data
        }

        // Vytvoření demo úkolů
        var tasks = new List<EvTask>
        {
            new EvTask
            {
                CompanyName = "ABC s.r.o.",
                Description = "Implementace nového modulu pro správu zákazníků",
                AssignedBy = "Jan Novák",
                AssignedTo = "Petr Svoboda",
                ReportedDate = DateTime.Now.AddDays(-10),
                Deadline = DateTime.Now.AddDays(20),
                Priority = Priority.High,
                Status = EvTaskStatus.InProgress,
                Communications = new List<ChatMessage>
                {
                    new ChatMessage
                    {
                        SenderName = "Jan Novák",
                        Message = "Prosím o analýzu současného stavu",
                        SendDate = DateTime.Now.AddDays(-10)
                    }
                },
                ChecklistItems = new List<ChecklistItem>
                {
                    new ChecklistItem
                    {
                        Description = "Analýza požadavků",
                        IsCompleted = true,
                        Deadline = DateTime.Now.AddDays(-5),
                        CompletedDate = DateTime.Now.AddDays(-6)
                    },
                    new ChecklistItem
                    {
                        Description = "Implementace backend",
                        IsCompleted = false,
                        Deadline = DateTime.Now.AddDays(10)
                    }
                }
            },
            new EvTask
            {
                CompanyName = "XYZ a.s.",
                Description = "Oprava chyby v reportingu",
                AssignedBy = "Marie Dvořáková",
                AssignedTo = "Jan Novák",
                ReportedDate = DateTime.Now.AddDays(-5),
                Deadline = DateTime.Now.AddDays(-1),
                Priority = Priority.Critical,
                Status = EvTaskStatus.New,
                Communications = new List<ChatMessage>
                {
                    new ChatMessage
                    {
                        SenderName = "Marie Dvořáková",
                        Message = "Urgentní oprava - reporting nefunguje správně",
                        SendDate = DateTime.Now.AddDays(-5)
                    }
                },
                ChecklistItems = new List<ChecklistItem>
                {
                    new ChecklistItem
                    {
                        Description = "Identifikace příčiny",
                        IsCompleted = false,
                        Deadline = DateTime.Now.AddDays(-2)
                    }
                }
            }
        };

        await context.EvTasks.AddRangeAsync(tasks);
        await context.SaveChangesAsync();
    }
}
