global using AwesomeAssertions;
global using AwesomeAssertions.Execution;
global using NSubstitute;
global using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.All)]
[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
