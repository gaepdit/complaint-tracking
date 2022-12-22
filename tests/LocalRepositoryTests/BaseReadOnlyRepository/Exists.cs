using Cts.LocalRepository.Repositories;

namespace LocalRepositoryTests.BaseReadOnlyRepository;

public class Exists
{
    private LocalOfficeRepository _repository = default!;

    [SetUp]
    public void SetUp() => _repository = new LocalOfficeRepository();

    [TearDown]
    public void TearDown() => _repository.Dispose();

    // TODO: AppLibraryExtra
}
