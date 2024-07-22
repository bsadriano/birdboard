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

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default
    )
    {
        UpdateTimestamp();

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
    }
}
