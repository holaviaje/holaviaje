using HolaViaje.Account.Data;
using HolaViaje.Account.Features.Identity;
using HolaViaje.Account.Features.Identity.Events;
using HolaViaje.Infrastructure.Messaging;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var devSpecificOrigins = "_devSpecificOrigins";

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ApplicationDbContext>(connectionName: "AccountDB", configureDbContextOptions: options =>
{
    options.UseOpenIddict();
});

builder.Services.AddMassTransit(x =>
{
    var kafkaBrokerServers = builder.Configuration.GetConnectionString("kafka");

    x.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });

    x.AddRider(rider =>
    {
        rider.AddProducer<UserRegisteredEvent>(IdentityEventConsts.AccountEventsTopic);
        rider.UsingKafka((context, k) => { k.Host(kafkaBrokerServers); });
    });
});

builder.Services.AddAuthorization();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

// Cionfigure OpenIddict
builder.Services.AddOpenIddict()
               .AddCore(coreOptions =>
               {
                   coreOptions.UseEntityFrameworkCore()
                                  .UseDbContext<ApplicationDbContext>();
               })
    // Register the OpenIddict server components.
    .AddServer(options =>
    {
        options
            .AllowPasswordFlow()
            .AllowClientCredentialsFlow()
            .AllowAuthorizationCodeFlow()
            .AllowRefreshTokenFlow();

        // Note: if you don't want to specify a client_id when sending
        // a token or revocation request, uncomment the following line.
        options.AcceptAnonymousClients();


        options
            .SetTokenEndpointUris("/connect/token")
            .SetAuthorizationEndpointUris("/connect/authorize")
            .SetUserInfoEndpointUris("/connect/userinfo")
            .SetEndSessionEndpointUris("/connect/endsession");

        // Encryption and signing of tokens
        options
            .AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate()
            .DisableAccessTokenEncryption();

        // Register scopes (permissions)
        options.RegisterScopes("api");
        options.RegisterScopes("profile");
        options.RegisterScopes("offline_access");

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        options
            .UseAspNetCore()
            .EnableTokenEndpointPassthrough()
            .EnableAuthorizationEndpointPassthrough()
            .EnableUserInfoEndpointPassthrough()
            .EnableEndSessionEndpointPassthrough()
            .DisableTransportSecurityRequirement();
    });

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure cross-origin requests
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: devSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7161",
                                              "http://localhost:5076").AllowAnyHeader().AllowAnyMethod();
                      });
});

builder.Services.AddMessaging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(devSpecificOrigins);

    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
