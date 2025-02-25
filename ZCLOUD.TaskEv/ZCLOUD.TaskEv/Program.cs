global using ZCLOUD.TaskEv.Components;
global using ZCLOUD.TaskEv.Core.Interfaces;
global using ZCLOUD.TaskEv.Core.Services;
global using ZCLOUD.TaskEv.Data;
global using ZCLOUD.TaskEv.Data.Data;
global using Microsoft.EntityFrameworkCore;
global using ZCLOUD.TaskEv.Data.Enums;
global using ZCLOUD.TaskEv.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// P�id�n� slu�eb do kontejneru
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Konfigurace datab�ze na z�klad� parametru z appsettings.json
var useInMemoryDb = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

if (useInMemoryDb)
{
    // Pou�it� InMemory datab�ze
    builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("TaskManagementDb")
    );
}
else
{
    // Pou�it� SQL Server datab�ze
    builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlOptions => sqlOptions.EnableRetryOnFailure()
        )
    );
}


// Registrace slu�by pro upload soubor�
builder.Services.AddScoped<IFileUploadService, FileUploadService>();

// Registrace slu�by u�ivatele
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();

// Konfigurace HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseWebAssemblyDebugging();
    //app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();
    

// Inicializace InMemory datab�ze a napln�n� demo daty
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Napln�me demo data
        await SeedData.Initialize(services, useInMemoryDb);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Chyba p�i inicializaci datab�ze.");
    }
}

app.Run();