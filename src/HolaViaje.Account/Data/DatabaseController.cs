using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HolaViaje.Account.Data;

[Route("[controller]")]
[ApiController]
public class DatabaseController : ControllerBase
{
    [HttpPost("migrate")]
    public async Task<IActionResult> Migrate()
    {
        using (var serviceScope = HttpContext.RequestServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();
        }
        return NoContent();
    }

    [HttpPost("initialize")]
    public async Task<IActionResult> Initialize()
    {
        using (var serviceScope = HttpContext.RequestServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        return NoContent();
    }
}
