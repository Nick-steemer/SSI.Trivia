using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using MudBlazor.Services;
using SSI.Trivia.Shared.DbContexts;
using SSI.Trivia.Shared.Hubs;
using SSI.Trivia.Shared.Services;
using SSI.Trivia.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add Microsoft Identity authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<AuthService>();

builder.Services.AddDbContext<TriviaDbContext>(options =>
{
    var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "SSI.Trivia.Shared", "Trivia.db");
    options.UseSqlite($"Filename={dbPath}");
});
builder.Services.AddSignalR();
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(SSI.Trivia.Shared._Imports).Assembly);

app.MapHub<TriviaHub>("/triviahub");
await app.RunAsync();
