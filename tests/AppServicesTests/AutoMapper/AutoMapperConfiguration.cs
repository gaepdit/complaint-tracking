using AutoMapper;
using AutoMapper.Internal;

namespace AppServicesTests.AutoMapper;

public class AutoMapperConfiguration
{
    [Test]
    public void MappingConfigurationsAreValid()
    {
        Setup.MapperConfiguration!.AssertConfigurationIsValid();
    }

    [Test]
    public void Detect_circular_type_maps()
    {
        // GHSA-rvv3-g6hj-g44x discussion
        // https://github.com/LuckyPennySoftware/AutoMapper/discussions/4624
        FindCircularMaps(Setup.MapperConfiguration!).Should().BeEmpty();
    }

    private static IEnumerable<TypeMap> FindCircularMaps(IConfigurationProvider config)
    {
        var allMaps = config.Internal().GetAllTypeMaps();
        var mapsByDestType = allMaps.ToLookup(tm => tm.DestinationType);
        return allMaps.Where(tm => IsCircular(tm, mapsByDestType, []));
    }

    private static bool IsCircular(TypeMap current, ILookup<Type, TypeMap> mapsByDestType, HashSet<TypeMap> path)
    {
        return !path.Add(current) ||
               current.MemberMaps
                   .Where(m => !m.Ignored)
                   .SelectMany(mm => mapsByDestType[mm.DestinationType])
                   .Any(nextMap => IsCircular(nextMap, mapsByDestType, [..path]));
    }
}
