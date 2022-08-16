using AutoMapper;

using Bogus;

namespace AutSoft.Linq.Tests.Queryable.OrderByExtensions;

public partial class OrderByExtensionsTests : IDisposable
{
    public OrderByExtensionsTests()
    {
        Faker = new Faker<Person>()
            .RuleFor(x => x.Id, f => f.IndexGlobal + 1)
            .RuleFor(x => x.Name, f => f.Person.FullName)
            .RuleFor(x => x.Address, f => f.Person.Address.ToString())
            .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth);

        Mapper = new MapperConfiguration(
            cfg => cfg.CreateMap<Person, PersonDto>()
                .ForMember(p => p.Age, opt => opt.MapFrom(p => (DateTimeOffset.UtcNow - p.DateOfBirth).Days / 365)))
            .CreateMapper();

        DbContext = new PersonDbContext();
        DbContext.People.AddRange(Faker.GenerateBetween(9, 21));
        DbContext.SaveChanges();

        Subject = DbContext.People;
    }

    private protected IQueryable<Person> Subject { get; set; }

    private protected Faker<Person> Faker { get; }
    private protected IMapper Mapper { get; }
    private protected PersonDbContext DbContext { get; }

    private bool _disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
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
