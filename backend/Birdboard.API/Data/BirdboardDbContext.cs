using Birdboard.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Birdboard.API.Data;

public class BirdboardDbContext : IdentityDbContext<AppUser>
{
    private List<EntityEntry> _addedProjects;
    private List<EntityEntry> _addedTasks;
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
        var changed = ChangeTracker.Entries();
        _addedProjects = GetEntries<Project>(EntityState.Added);

        _addedTasks = GetEntries<ProjectTask>(EntityState.Added);

        // TODO: Refactor to separate interceptors
        UpdateTimestamp();
        RecordUpdateActivity();
        RecordDeletedTaskActivity();

        int result = await base.SaveChangesAsync(cancellationToken);
        RecordAddActivity();
        RecordAddTaskActivity();

        _addedProjects.Clear();

        return result;
    }

    private List<EntityEntry> GetEntries<T>(EntityState state)
    {
        return ChangeTracker.Entries()
            .Where(e => e.State == state && e.Entity is T)
            .ToList();
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
        foreach (var project in _addedProjects.Select(e => (Project)e.Entity).ToList())
        {
            Activities.Add(new Activity
            {
                SubjectId = project.Id,
                Description = "created",
                SubjectType = "Project",
            });
        }

        base.SaveChanges();
    }

    private void RecordUpdateActivity()
    {
        var projectsModified = ChangeTracker.Entries<Project>()
            .Where(e => e.State == EntityState.Modified);

        foreach (EntityEntry<Project> entityEntry in projectsModified)
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
                Activities.Add(new Activity
                {
                    Description = "updated",
                    SubjectId = entityEntry.Property(a => a.Id).CurrentValue,
                    SubjectType = "Project"
                });
            }
        }

        var tasksModified = ChangeTracker.Entries<ProjectTask>()
            .Where(e => e.State == EntityState.Modified);

        foreach (EntityEntry<ProjectTask> entityEntry in tasksModified)
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

            if (differences.Count == 2 && differences.ContainsKey("Completed"))
            {
                Activities.Add(new Activity
                {
                    Description = entityEntry.Property(a => a.Completed).CurrentValue ? "completed_task" : "incompleted_task",
                    SubjectId = entityEntry.Property(a => a.Id).CurrentValue,
                    SubjectType = "ProjectTask"
                });
            }
            else if (!(differences.Count == 1 && differences.ContainsKey("UpdatedAt")))
            {
                Activities.Add(new Activity
                {
                    Description = "updated",
                    SubjectId = entityEntry.Property(a => a.Id).CurrentValue,
                    SubjectType = "ProjectTask"
                });
            }
        }
    }

    private void RecordAddTaskActivity()
    {
        var tasksAdded = _addedTasks
            .Select(e => (ProjectTask)e.Entity);

        foreach (var task in tasksAdded)
        {
            Activities.Add(new Activity
            {
                Description = "created_task",
                SubjectId = task.Id,
                SubjectType = "ProjectTask"
            });
        }

        base.SaveChanges();
    }

    private void RecordDeletedTaskActivity()
    {
        var tasksDeleted = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted)
            .Select(e => e.Entity)
            .OfType<ProjectTask>();

        foreach (var task in tasksDeleted)
        {
            Activities.Add(new Activity
            {
                Description = "deleted_task",
                SubjectId = task.Id,
                SubjectType = "ProjectTask"
            });
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProjectTask>()
            .HasOne(o => o.Project)
            .WithMany(p => p.Tasks)
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

        // builder.Entity<Project>()
        //     .HasDiscriminator<string>("Discriminator")
        //     .HasValue<Project>("Project");

        // builder.Entity<Activity>()
        //     .HasKey(c => c.Id);

        // builder.Entity<Activity>()
        //     .Property(c => c.SubjectType)
        //     .IsRequired();

        // builder.Entity<Activity>()
        //     .Property(c => c.SubjectId)
        //     .IsRequired();
    }
}
