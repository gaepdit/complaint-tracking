﻿using System.Diagnostics;
using Cts.TestData;
using FluentAssertions.Execution;

namespace IntegrationTests.BaseReadOnlyRepository;

public class FindByPredicateSqlServer
{
    [Test]
    public async Task SqlServerDatabaseIsNotCaseSensitive()
    {
        using var repositoryHelper = RepositoryHelper.CreateSqlServerRepositoryHelper(this);
        using var repository = repositoryHelper.GetOfficeRepository();
        var item = OfficeData.GetOffices.First(e => e.Active);

        // Test using a predicate that compares uppercase names.
        var resultSameCase = await repository.FindAsync(e =>
            e.Name.ToUpper().Equals(item.Name.ToUpper()));

        // Test using a predicate that compares an uppercase name to a lowercase name.
        var resultDifferentCase = await repository.FindAsync(e =>
            e.Name.ToUpper().Equals(item.Name.ToLower()));

        using (new AssertionScope())
        {
            resultSameCase.Should().BeEquivalentTo(item, opts =>
                opts.Excluding(e => e.StaffMembers));
            resultDifferentCase.Should().BeEquivalentTo(item, opts =>
                opts.Excluding(e => e.StaffMembers));
        }
    }
}
