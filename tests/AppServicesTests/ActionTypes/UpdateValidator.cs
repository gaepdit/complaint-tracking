﻿using Cts.AppServices.ActionTypes;
using Cts.Domain.ActionTypes;
using Cts.Domain.Entities;
using Cts.TestData.ActionTypes;
using FluentValidation.TestHelper;

namespace AppServicesTests.ActionTypes;

public class UpdateValidator
{
    [Test]
    public async Task ValidDto_ReturnsAsValid()
    {
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ActionType?)null);
        var model = new ActionTypeUpdateDto
        {
            Id = Guid.Empty,
            Name = ActionTypeConstants.ValidName,
            Active = true,
        };

        var validator = new ActionTypeUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task DuplicateName_ReturnsAsInvalid()
    {
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ActionType(Guid.NewGuid(), ActionTypeConstants.ValidName));
        var model = new ActionTypeUpdateDto
        {
            Id = Guid.Empty,
            Name = ActionTypeConstants.ValidName,
            Active = true,
        };

        var validator = new ActionTypeUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name)
            .WithErrorMessage("The name entered already exists.");
    }

    [Test]
    public async Task DuplicateName_ForSameId_ReturnsAsValid()
    {
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ActionType(Guid.Empty, ActionTypeConstants.ValidName));
        var model = new ActionTypeUpdateDto
        {
            Id = Guid.Empty,
            Name = ActionTypeConstants.ValidName,
            Active = true,
        };

        var validator = new ActionTypeUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(e => e.Name);
    }

    [Test]
    public async Task NameTooShort_ReturnsAsInvalid()
    {
        var repoMock = new Mock<IActionTypeRepository>();
        repoMock.Setup(l => l.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ActionType?)null);
        var model = new ActionTypeUpdateDto() { Name = ActionTypeConstants.ShortName };

        var validator = new ActionTypeUpdateValidator(repoMock.Object);
        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(e => e.Name);
    }
}
