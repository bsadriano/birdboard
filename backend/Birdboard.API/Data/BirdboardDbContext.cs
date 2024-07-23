using Birdboard.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        GenerateAddActivity();
        GenerateUpdateActivity();

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

    private void GenerateAddActivity()
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

    private void GenerateUpdateActivity()
    {
        var projectsAdded = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified)
            .Select(e => e.Entity)
            .OfType<Project>();

        foreach (var project in projectsAdded)
        {
            project.Activities.Add(new Activity
            {
                Description = "updated"
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
