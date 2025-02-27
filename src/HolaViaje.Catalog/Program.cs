using HolaViaje.Catalog;
using HolaViaje.Catalog.Data;
using HolaViaje.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var devSpecificOrigins = "_devSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ApplicationDbContext>(connectionName: "CatalogDB");
builder.AddAzureBlobClient("catalogBlobs");

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure cross-origin requests
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: devSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7281",
                                              "http://localhost:5194").AllowAnyHeader().AllowAnyMethod();
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

builder.Services.AddCatalogServices();
builder.Services.AddInfrastructure();

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
