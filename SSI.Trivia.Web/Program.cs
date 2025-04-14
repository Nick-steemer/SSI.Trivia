using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using SSI.Trivia.Shared.DbContexts;
using SSI.Trivia.Shared.Hubs;
using SSI.Trivia.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(SSI.Trivia.Shared._Imports).Assembly);

app.MapHub<TriviaHub>("/triviahub");
await app.RunAsync();
