var builder = WebApplication.CreateBuilder(args);

var devSpecificOrigins = "_devSpecificOrigins";

builder.AddServiceDefaults();

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
