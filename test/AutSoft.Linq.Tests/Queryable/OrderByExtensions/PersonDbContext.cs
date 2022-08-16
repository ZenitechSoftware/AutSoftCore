using Microsoft.EntityFrameworkCore;

namespace AutSoft.Linq.Tests.Queryable.OrderByExtensions;

internal class PersonDbContext : DbContext
{
    public DbSet<Person> People => Set<Person>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Every DbContext instance works on a different in-memory db
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

        base.OnConfiguring(optionsBuilder);
    }
}
