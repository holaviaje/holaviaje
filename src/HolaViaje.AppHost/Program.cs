using Projects;

var builder = DistributedApplication.CreateBuilder(args);

#region Messaging
var kafka = builder.AddKafka("kafka")
    .WithDataVolume(isReadOnly: false)
    .WithKafkaUI();
#endregion

#region Azure Storage

var socialStorage = builder.AddAzureStorage("socialStorage")
    .RunAsEmulator(azurite =>
    {
        azurite.WithImage("azure-storage/azurite", "3.33.0");
        azurite.WithDataVolume();
    });

var socialBlobs = socialStorage.AddBlobs("socialBlobs");
var catalogBlobs = socialStorage.AddBlobs("CatalogBlobs");

#endregion

#region PostgreSQL

// Parameters for the builder
var dbPassword = builder.AddParameter("DbPassword", true);

// Add PostgreSQL to the container.
var postgresql = builder.AddPostgres("postgresql-server", password: dbPassword);

var accountDb = postgresql.AddDatabase("AccountDB");
var socialDb = postgresql.AddDatabase("SocialDB");
var catalogDb = postgresql.AddDatabase("CatalogDB");

postgresql.WithDataVolume().WithPgAdmin();

#endregion

builder.AddProject<HolaViaje_Account>("account-api")
    .WithReference(accountDb)
    .WaitFor(accountDb)
    .WithReference(kafka)
    .WaitFor(kafka);

builder.AddProject<HolaViaje_Social>("social-api")
    .WithReference(socialDb)
    .WaitFor(socialDb)
    .WithReference(socialBlobs)
    .WaitFor(socialStorage)
    .WithReference(kafka)
    .WaitFor(kafka);

builder.AddProject<HolaViaje_Catalog>("catalog-api")
    .WithReference(catalogBlobs)
    .WaitFor(catalogBlobs)
    .WithReference(catalogDb)
    .WaitFor(catalogDb)
    .WithReference(kafka)
    .WaitFor(kafka);

builder.Build().Run();
