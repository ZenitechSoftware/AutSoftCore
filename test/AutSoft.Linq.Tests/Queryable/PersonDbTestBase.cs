using Bogus;

namespace AutSoft.Linq.Tests.Queryable;

public class PersonDbTestBase : IDisposable
{
    public PersonDbTestBase()
    {
        Faker = new Faker<Person>()
            .RuleFor(x => x.Id, f => f.IndexGlobal + 1)
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Address, f => f.Person.Address.ToString())
            .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth);

        DbContext = new PersonDbContext();
    }

    protected Faker<Person> Faker { get; }
    private protected PersonDbContext DbContext { get; }

    private bool _disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                DbContext.Database.EnsureDeleted();
                DbContext.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
