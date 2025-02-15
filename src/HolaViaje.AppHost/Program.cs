using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<HolaViaje_Account>("account-api");

builder.Build().Run();
