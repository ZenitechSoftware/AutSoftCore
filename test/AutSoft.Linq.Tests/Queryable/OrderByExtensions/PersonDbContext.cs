using Microsoft.EntityFrameworkCore;

namespace AutSoft.Linq.Tests.Queryable.OrderByExtensions;

internal class PersonDbContext : DbContext
{
    public DbSet<Person> People => Set<Person>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("test");

        base.OnConfiguring(optionsBuilder);
    }
}
