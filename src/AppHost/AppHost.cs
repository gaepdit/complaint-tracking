var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.WebApp>(nameof(Projects.WebApp));

await builder.Build().RunAsync();
