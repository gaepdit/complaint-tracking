using FluentAssertions.Extensions;
using System.Diagnostics;

namespace EfRepositoryTests;

[SetUpFixture]
public class EfRepositoryTestsSetup
{
    [OneTimeSetUp]
    public void RunBeforeAllTests()
    {
        AssertionOptions.AssertEquivalencyUsing(opts => opts
            // DateTimeOffset comparison is often off by a few microseconds.
            .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 10.Milliseconds()))
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
