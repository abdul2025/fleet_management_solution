using FleetManagement.Application.Aircrafts.Handlers;
using FleetManagement.Application.Aircrafts.Interfaces;
using FleetManagement.Domain.CommonEntities;
using FleetManagement.Infrastructure.Data;
using FleetManagement.Infrastructure.Data.Interceptors;
using FleetManagement.Infrastructure.Services.Aircrafts;
using FleetManagement.Infrastructure.Services.Shared;
using FleetManagement.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// Add Services to DI Container
// ==========================

// MVC Controllers + Views
builder.Services.AddControllersWithViews();

// --------------------------
// Email Service
// --------------------------
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);
builder.Services.AddSingleton<IEmailService, EmailService>(); // or MockEmailService for testing

// --------------------------
// Domain Event Dispatcher
// --------------------------
builder.Services.AddSingleton<DomainEventDispatcher>(sp =>
    new DomainEventDispatcher(sp) // Pass IServiceProvider to resolve handlers
);

// --------------------------
// Application Handlers
// --------------------------
builder.Services.AddTransient<AircraftCreatedHandler>();

// --------------------------
// Aircraft Service
// --------------------------
builder.Services.AddScoped<IAircraftService, AircraftService>();

// --------------------------
// DbContext with Interceptor
// --------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("FleetManagement.Infrastructure")
        )
        .AddInterceptors(new BaseEntityInterceptor())
);

// ==========================
// Build App
// ==========================
var app = builder.Build();

// ==========================
// Middleware Pipeline
// ==========================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets(); // If using static assets
app.MapControllers(); // Required for attribute routing

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// ==========================
// Apply Pending Migrations & Optional Seed Data
// ==========================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Apply migrations automatically
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
        
        // Optional: Seed initial data
        // await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

// ==========================
// Run the App
// ==========================
app.Run();
