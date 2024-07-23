using Birdboard.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Birdboard.API.Data;

public class BirdboardDbContext : IdentityDbContext<AppUser>
{
    public BirdboardDbContext(DbContextOptions dbContextOptions)
    : base(dbContextOptions)
    {

    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<Activity> Activities { get; set; }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default
    )
    {
        // TODO: Refactor to separate interceptors
        UpdateTimestamp();
        RecordAddActivity();
        RecordUpdateActivity();
        RecordAddTaskActivity();
        RecordCompletedTaskActivity();

        int result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    private void UpdateTimestamp()
    {
        var entitesToUpdate = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified)
            .Select(e => e.Entity)
            .OfType<BaseEntity>();

        foreach (var entity in entitesToUpdate)
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    private void RecordAddActivity()
    {
        var projectsAdded = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity)
            .OfType<Project>();

        foreach (var project in projectsAdded)
        {
            project.Activities.Add(new Activity
            {
                Description = "created"
            });
        }
    }

    private void RecordUpdateActivity()
    {
        var projectsAdded = ChangeTracker.Entries<Project>()
            .Where(e => e.State == EntityState.Modified);

        foreach (EntityEntry<Project> entityEntry in projectsAdded)
        {
            var props = entityEntry.CurrentValues.Properties;
            var currentValues = entityEntry.CurrentValues;
            var originalValues = entityEntry.GetDatabaseValues();

            var differences = new Dictionary<string, Tuple<object, object>>();

            foreach (var property in props)
            {
                var currentValue = currentValues[property];
                var originalValue = originalValues[property];

                if (!Equals(currentValue, originalValue))
                {
                    differences.Add(property.Name, new Tuple<object, object>(originalValue, currentValue));
                }
            }

            if (differences.Count != 1 || !differences.ContainsKey("UpdatedAt"))
            {
                this.Activities.Add(new Activity
                {
                    ProjectId = entityEntry.Property(a => a.Id).CurrentValue,
                    Description = "updated"
                });
            }

        }
    }

    private void RecordAddTaskActivity()
    {
        var tasksAdded = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity)
            .OfType<ProjectTask>();

        foreach (var task in tasksAdded)
        {
            task.Project.Activities.Add(new Activity
            {
                Description = "created_task"
            });
        }
    }

    private void RecordCompletedTaskActivity()
    {
        var tasksModified = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified)
            .Select(e => e.Entity)
            .OfType<ProjectTask>();

        foreach (var task in tasksModified)
        {
            if (task.Completed)
            {
                task.Project.Activities.Add(new Activity
                {
                    Description = "completed_task"
                });
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProjectTask>()
            .HasOne(o => o.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(p => p.ProjectId);

        builder.Entity<Activity>()
            .HasOne(o => o.Project)
            .WithMany(p => p.Activities)
            .HasForeignKey(p => p.ProjectId);

        List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}
