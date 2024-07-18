using Birdboard.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Birdboard.API.Data;

public class BirdboardDbContext : DbContext
{
    public BirdboardDbContext(DbContextOptions dbContextOptions)
    : base(dbContextOptions)
    {

    }

    public DbSet<Project> Projects { get; set; }
}
