namespace AppServicesTests.AutoMapper;

public class AutoMapperConfiguration
{
    [Test]
    public void MappingConfigurationsAreValid()
    {
        AppServicesTestsSetup.MapperConfig!.AssertConfigurationIsValid();
    }
}
