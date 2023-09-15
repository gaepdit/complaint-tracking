using Cts.LocalRepository.Identity;
using Cts.LocalRepository.Repositories;
using Cts.TestData;
using Cts.TestData.Identity;

namespace LocalRepositoryTests;

public static class RepositoryHelper
{
    public static LocalConcernRepository GetConcernRepository()
    {
        ClearAllStaticData();
        return new LocalConcernRepository();
    }

    public static LocalOfficeRepository GetOfficeRepository()
    {
        ClearAllStaticData();
        return new LocalOfficeRepository();
    }

    public static LocalUserStore GetLocalUserStore()
    {
        ClearAllStaticData();
        return new LocalUserStore();
    }

    private static void ClearAllStaticData()
    {
        ConcernData.ClearData();
        OfficeData.ClearData();
        UserData.ClearData();
    }
}
