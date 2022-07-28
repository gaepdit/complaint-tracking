using AutoMapper;
using Cts.Application.ActionTypes;
using Cts.Application.Offices;

namespace ApplicationTests;

[SetUpFixture]
public class Global
{
    internal static IMapper? Mapper;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile(new ActionTypeMappingProfile());
            c.AddProfile(new OfficeMappingProfile());
        });

        Mapper = mapperConfig.CreateMapper();
    }
}
