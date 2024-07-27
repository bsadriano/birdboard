using Birdboard.API.Mappers;
using Newtonsoft.Json.Serialization;
using Birdboard.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Birdboard.API.Services.UserService;

namespace Birdboard.API.Data;

public class BirdboardDbContext : IdentityDbContext<AppUser>
{
    private List<EntityEntry> _addedProjects;
    private List<EntityEntry> _addedTasks;
    private JsonSerializerSettings settings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        PreserveReferencesHandling = PreserveReferencesHandling.None,
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Formatting = Formatting.Indented // Optional, for better readability
    };
    private readonly IUserService _userService;

    public BirdboardDbContext(DbContextOptions dbContextOptions, IUserService userService)
    : base(dbContextOptions)
    {
        _userService = userService;
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    public DbSet<Activity> Activities { get; set; }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default
    )
    {
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
            RecordActivity(project, "created");
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

                if (!Equals(currentValue, originalValue) && property.Name != "UpdatedAt")
                {
                    differences.Add(property.Name, new Tuple<object, object>(originalValue, currentValue));
                }
            }

            if (differences.Count > 0)
            {
                RecordActivity(entityEntry.Entity, "updated", differences);
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

                if (!Equals(currentValue, originalValue) && property.Name != "UpdatedAt")
                {
                    differences.Add(property.Name, new Tuple<object, object>(originalValue, currentValue));
                }
            }

            if (differences.Count == 1 && differences.ContainsKey("Completed"))
            {
                RecordActivity(entityEntry.Entity, entityEntry.Entity.Completed ? "completed_task" : "incompleted_task");
            }
            else if (differences.Count > 0)
            {
                RecordActivity(entityEntry.Entity, "updated_task");
            }
        }
    }

    private void RecordAddTaskActivity()
    {
        var tasksAdded = _addedTasks
            .Select(e => (ProjectTask)e.Entity);

        foreach (var task in tasksAdded)
        {
            RecordActivity(task, "created_task");
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
            RecordActivity(task, "deleted_task");
        }
    }

    private void RecordActivity(Project project, string description, Dictionary<string, Tuple<object, object>>? differences = null)
    {
        Activities.Add(new Activity
        {
            SubjectId = project.Id,
            ProjectId = project.Id,
            Description = description,
            SubjectType = "Project",
            EntityData = JsonConvert.SerializeObject(project.ToProjectDto(), settings),
            Changes = ActivityChanges(description, differences),
            UserId = project.OwnerId
        });
    }

    private void RecordActivity(ProjectTask task, string description)
    {
        Activities.Add(new Activity
        {
            ProjectId = task.ProjectId,
            Description = description,
            SubjectId = task.Id,
            SubjectType = "ProjectTask",
            EntityData = JsonConvert.SerializeObject(task.ToProjectTaskDto(), settings),
            UserId = task.Project.OwnerId
        });
    }

    private string? ActivityChanges(string description, Dictionary<string, Tuple<object, object>>? differences)
    {
        if (description != "updated" || differences == null || differences.Count == 0)
        {
            return null;
        }

        var changes = new
        {
            Before = differences.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Item1
            ),
            After = differences.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Item2
            ),
        };

        return JsonConvert.SerializeObject(changes, settings);
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProjectTask>()
            .HasOne(o => o.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(p => p.ProjectId);

        builder.Entity<Activity>()
            .HasOne(a => a.Project)
            .WithMany(p => p.Activities)
            .HasForeignKey(p => p.ProjectId);

        builder.Entity<Activity>()
            .HasOne(a => a.User)
            .WithMany(u => u.Activities)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

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
