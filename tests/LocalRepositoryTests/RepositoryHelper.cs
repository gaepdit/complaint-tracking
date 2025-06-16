using Cts.LocalRepository.Identity;
using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests;

public static class RepositoryHelper
{
    public static LocalOfficeRepository GetOfficeRepository() => new();
    public static LocalUserStore GetLocalUserStore() => new();
}
