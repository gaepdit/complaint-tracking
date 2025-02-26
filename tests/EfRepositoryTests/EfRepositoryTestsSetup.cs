using FluentAssertions.Extensions;
using System.Diagnostics;

namespace EfRepositoryTests;

[SetUpFixture]
public class EfRepositoryTestsSetup
{
    [OneTimeSetUp]
    public void RunBeforeAllTests()
    {
        AssertionConfiguration.Current.Equivalency.Modify(options => options
            // DateTimeOffset comparison is often off by a few microseconds.
            .Using<DateTimeOffset>(
                context => context.Subject.Should().BeCloseTo(context.Expectation, 10.Milliseconds()))
            .WhenTypeIs<DateTimeOffset>()
        );
    }

    [OneTimeTearDown]
    public void RunAfterAllTests()
    {
        // Don't leave LocalDB process running (fixes test runner warning)
        // See https://resharper-support.jetbrains.com/hc/en-us/community/posts/360006736719/comments/360002383960
        using var process = Process.Start("sqllocaldb", "stop MSSQLLocalDB");
        process.WaitForExit();
    }
}
