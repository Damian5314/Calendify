using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StarterKit.Models;
using StarterKit.Services;

var builder = WebApplication.CreateBuilder(args);

// Voeg session storage toe
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15); // Sessietijd instellen
    options.Cookie.HttpOnly = true; // Voorkomt JavaScript-toegang
    options.Cookie.IsEssential = true; // Vereist voor GDPR
});

// Voeg Cookie Authentication toe
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/v1/auth/login"; // Correcte login route
        options.LogoutPath = "/api/v1/auth/logout";
        options.AccessDeniedPath = "/api/v1/auth/access-denied"; 
        options.Cookie.HttpOnly = true; // Beveiliging tegen JavaScript-aanvallen
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.None  // HTTP toegestaan tijdens ontwikkeling
            : CookieSecurePolicy.Always; // Alleen HTTPS in productie
    });

// Voeg autorisatie toe
builder.Services.AddAuthorization();

// Voeg controllers en database toe
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqlLiteDb")));

// Voeg aangepaste services toe
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEventStorage, DbEventStorage>();

// JSON-opties instellen
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Voeg CORS ondersteuning toe
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Belangrijk voor cookies!
    });
});

var app = builder.Build();

// Configureer de HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// CORS inschakelen
app.UseCors("AllowFrontend");

app.UseRouting();

// **Zet UseSession vóór UseAuthentication**
app.UseSession();

// Voeg authenticatie en autorisatie middleware toe
app.UseAuthentication();
app.UseAuthorization();

// Voeg routes toe
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
