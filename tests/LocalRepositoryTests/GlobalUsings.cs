global using FluentAssertions;
global using FluentAssertions.Execution;
global using NSubstitute;
global using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.All)]
[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
