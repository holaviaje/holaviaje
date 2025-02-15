using HolaViaje.Account.Data;
using HolaViaje.Account.Features.Identity;

var builder = WebApplication.CreateBuilder(args);

var devSpecificOrigins = "_devSpecificOrigins";

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ApplicationDbContext>(connectionName: "AccountDB", configureDbContextOptions: options => { });

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
