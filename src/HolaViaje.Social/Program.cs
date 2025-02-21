using HolaViaje.Account.Features.Identity.Events;
using HolaViaje.Social;
using HolaViaje.Social.Data;
using HolaViaje.Social.IntegrationEvents.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;

var devSpecificOrigins = "_devSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ApplicationDbContext>(connectionName: "SocialDB");
builder.AddAzureBlobClient("socialBlobs");

builder.Services.AddMassTransit(x =>
{
    const string accountEventsGroup = "events-account-group-1";
    var kafkaBrokerServers = builder.Configuration.GetConnectionString("kafka");

    x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

    x.AddRider(rider =>
    {
        rider.AddConsumer<IdentityEventsConsumer>();
        rider.UsingKafka((context, k) =>
        {
            k.Host(kafkaBrokerServers);

            k.TopicEndpoint<UserRegisteredEvent>(IdentityEventConsts.AccountEventsTopic, accountEventsGroup, e =>
            {
                e.ConfigureConsumer<IdentityEventsConsumer>(context);
                e.CreateIfMissing();
            });
        });
    });
});

builder.Services.AddControllers();
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure cross-origin requests
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: devSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7089",
                                              "http://localhost:5128").AllowAnyHeader().AllowAnyMethod();
                      });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        // base-address of Auth Server
        options.Authority = "https://localhost:7161/";

        // name of the API resource
        options.Audience = "Resourse";

        options.RequireHttpsMetadata = false;

        // Check preferred_username claim exists in the token. If it exists, .NET Core framework sets it to currently logged-in user name i-e User.Identity.Name
        options.TokenValidationParameters.NameClaimType = "preferred_username";
        options.TokenValidationParameters.RoleClaimType = System.Security.Claims.ClaimTypes.Role;
    });

builder.Services.AddSocialServices();

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
