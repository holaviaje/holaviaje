using Projects;

var builder = DistributedApplication.CreateBuilder(args);

#region PostgreSQL

// Parameters for the builder
var dbPassword = builder.AddParameter("DbPassword", true);

// Add PostgreSQL to the container.
var postgresql = builder.AddPostgres("postgresql-server", password: dbPassword);

var accountDb = postgresql.AddDatabase("AccountDB");

postgresql.WithDataVolume().WithPgAdmin();

#endregion

builder.AddProject<HolaViaje_Account>("account-api")
    .WithReference(accountDb)
    .WaitFor(accountDb);

builder.Build().Run();
