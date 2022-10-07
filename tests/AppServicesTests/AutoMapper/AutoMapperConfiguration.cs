namespace AppServicesTests.AutoMapper;

public class AutoMapperConfiguration
{
    [Test]
    public void MappingConfigurationsAreValid()
    {
        AppServicesTestsGlobal.MapperConfig!.AssertConfigurationIsValid();
    }
}
