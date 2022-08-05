using AutoMapper;
using Cts.AppServices.ActionTypes;
using Cts.Domain.ActionTypes;
using Cts.TestData.ActionTypes;
using GaEpd.Library.Domain.Repositories;

namespace AppServicesTests.ActionTypes;

public class Get
{
    [Test]
    public async Task WhenItemExists_ReturnsViewDto()
    {
        var item = new ActionType(Guid.Empty, ActionTypeConstants.ValidName);
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.GetAsync(item.Id, default))
            .ReturnsAsync(item);
        var managerMock = new Mock<IActionTypeManager>();
        var appService = new ActionTypeAppService(repoMock.Object, managerMock.Object, AppServicesTestsGlobal.Mapper!);

        var result = await appService.GetAsync(Guid.Empty);

        result.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task WhenDoesNotExist_Throws()
    {
        var id = Guid.Empty;
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.GetAsync(id, default))
            .ThrowsAsync(new EntityNotFoundException(typeof(ActionType), id));
        var managerMock = new Mock<IActionTypeManager>();
        var mapperMock = new Mock<IMapper>();
        var appService = new ActionTypeAppService(repoMock.Object, managerMock.Object, mapperMock.Object);

        var action = async () => await appService.GetAsync(Guid.Empty);

        (await action.Should().ThrowAsync<EntityNotFoundException>())
            .WithMessage($"Entity not found. Entity type: {typeof(ActionType).FullName}, id: {id}");
    }
}
