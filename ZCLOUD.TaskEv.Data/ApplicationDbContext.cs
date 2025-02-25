
using Microsoft.EntityFrameworkCore;
using ZCLOUD.TaskEv.Data.Models;

namespace ZCLOUD.TaskEv.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }

    public DbSet<EvTask> EvTasks { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<ChecklistItem> ChecklistItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EvTask>()
            .HasMany(t => t.Communications)
            .WithOne(c => c.Task)
            .HasForeignKey(c => c.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EvTask>()
            .HasMany(t => t.Documents)
            .WithOne(d => d.Task)
            .HasForeignKey(d => d.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EvTask>()
            .HasMany(t => t.ChecklistItems)
            .WithOne(ci => ci.Task)
            .HasForeignKey(ci => ci.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
