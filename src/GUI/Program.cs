using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;

using Microsoft.EntityFrameworkCore;

using Serilog;
using Serilog.Core;

using DataAccess;
using BusinessLogic.Services;
using BusinessLogic.IRepositories;
using DataAccess.Repositories;

Log.Logger = new LoggerConfiguration()  
                 .Enrich.FromLogContext()  
                 .MinimumLevel.Debug()
                 .WriteTo.File(@"log.txt")
                 .CreateLogger();

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

builder.Services.AddSingleton<CurUserService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IBoardGameService, BoardGameService>();
builder.Services.AddTransient<IBoardGameEventService, BoardGameEventService>();
builder.Services.AddTransient<IOrganizerService, OrganizerService>();
builder.Services.AddTransient<IVenueService, VenueService>();
builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<IProducerService, ProducerService>();
builder.Services.AddTransient<IEncryptionService, BCryptEntryptionService>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IBoardGameRepository, BoardGameRepository>();
builder.Services.AddTransient<IBoardGameEventRepository, BoardGameEventRepository>();
builder.Services.AddTransient<IOrganizerRepository, OrganizerRepository>();
builder.Services.AddTransient<IVenueRepository, VenueRepository>();
builder.Services.AddTransient<IPlayerRepository, PlayerRepository>();
builder.Services.AddTransient<IProducerRepository, ProducerRepository>();

builder.Configuration.AddJsonFile("dbsettings.json");

var provider = builder.Configuration["Database"];
builder.Services.AddDbContext<BGEContext>(
    options => _ = provider switch
    {
        "Postgres" => options.UseNpgsql(builder.Configuration
                                        .GetConnectionString("DefaultConnection")),
        "MySQL" => options.UseMySql(builder.Configuration.GetConnectionString("MySQL"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQL"))),
        _ => throw new Exception($"Unsupported provider: {provider}")
    }
); 

if (provider == "Postgres")
{
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
}

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

Log.Information("App has been started");
app.Run();
