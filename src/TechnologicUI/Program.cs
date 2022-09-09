using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;

using Microsoft.EntityFrameworkCore;

using DataAccess;
using BusinessLogic.Services;
using BusinessLogic.IRepositories;
using DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

// maybe scoped???
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddTransient<IBoardGameService, BoardGameService>();
builder.Services.AddTransient<IBoardGameEventService, BoardGameEventService>();
builder.Services.AddTransient<IOrganizerService, OrganizerService>();
builder.Services.AddTransient<IVenueService, VenueService>();
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddSingleton<IEncryptionService, BCryptEntryptionService>();

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddTransient<IBoardGameRepository, BoardGameRepository>();
builder.Services.AddTransient<IBoardGameEventRepository, BoardGameEventRepository>();
builder.Services.AddTransient<IOrganizerRepository, OrganizerRepository>();
builder.Services.AddTransient<IVenueRepository, VenueRepository>();
builder.Services.AddTransient<IPlayerRepository, PlayerRepository>();

builder.Configuration.AddJsonFile("dbsettings.json");
builder.Services.AddDbContext<BGEContext>(options => options.UseNpgsql(
      builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
