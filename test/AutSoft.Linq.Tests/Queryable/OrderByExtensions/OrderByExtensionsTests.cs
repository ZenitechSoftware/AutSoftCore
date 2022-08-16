using AutoMapper;

using Bogus;

namespace AutSoft.Linq.Tests.Queryable.OrderByExtensions;

public partial class OrderByExtensionsTests : PersonDbTestBase
{
    public OrderByExtensionsTests()
    {
        Mapper = new MapperConfiguration(
            cfg => cfg.CreateMap<Person, PersonDto>()
                .ForMember(p => p.Age, opt => opt.MapFrom(p => (DateTimeOffset.UtcNow - p.DateOfBirth).Days / 365)))
            .CreateMapper();

        DbContext.People.AddRange(Faker.GenerateBetween(9, 21));
        DbContext.SaveChanges();

        Subject = DbContext.People;
    }

    private IQueryable<Person> Subject { get; set; }

    private IMapper Mapper { get; }
}
