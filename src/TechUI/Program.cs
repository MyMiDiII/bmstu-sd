using BusinessLogic.Services;
using BusinessLogic.Models;
using BusinessLogic.IRepositories;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using DataAccess;
using Npgsql.EntityFrameworkCore.PostgreSQL;

Console.WriteLine("START");

Console.WriteLine("[ INFO  ] BEGIN: Configuration");
var configuration = new ConfigurationBuilder().AddJsonFile("dbsettings.json");
var config = configuration.Build();
var connectionString = config.GetConnectionString("DefaultConnection");

var optionsBuilder = new DbContextOptionsBuilder<BGEContext>();
optionsBuilder.UseNpgsql(connectionString);
Console.WriteLine("[ INFO  ] END: Configuration");

Console.WriteLine("[ INFO  ] BEGIN: Creating services and repos");
BGEContext context = new BGEContext(optionsBuilder.Options);

IBoardGameEventRepository boardGameEventRepository = new BoardGameEventRepository(context);
IBoardGameRepository boardGameRepository = new BoardGameRepository(context);
IOrganizerRepository organizerRepository = new OrganizerRepository(context);
IVenueRepository venueRepository = new VenueRepository(context);
IUserRepository userRepository = new UserRepository(context);
IPlayerRepository playerRepository = new PlayerRepository(context);

IBoardGameEventService boardGameEventService = new BoardGameEventService(
                                                   boardGameEventRepository,
                                                   organizerRepository,
                                                   venueRepository);
CurUserService curUserService = new CurUserService();
IEncryptionService encryptionService = new BCryptEntryptionService();
IUserService userService = new UserService(userRepository, curUserService, encryptionService);
IPlayerService playerService = new PlayerService(playerRepository, userService);
IBoardGameService boardGameService = new BoardGameService(boardGameRepository, playerService);
IOrganizerService organizerService = new OrganizerService(organizerRepository, userService);
Console.WriteLine("[ INFO  ] END: Creating services and repos");

Console.WriteLine("[ TEST1 ] Get Events");

try
{
    var events = boardGameEventService.GetBoardGameEvents();
    Console.WriteLine("[ INFO  ] First event name: {0}", events[0].Title);
    Console.WriteLine("[ TEST1 ] OK");
}
catch(Exception ex)
{
    Console.WriteLine("[ ERROR ] {0}", ex.GetType());
    Console.WriteLine("[ TEST1 ] FAILED");
}

Console.WriteLine("[ TEST2 ] Registration");

long userID = userService.GetUserByName("newuser")?.ID ?? 0;

try
{
    userService.Register(new RegisterRequest("newuser", "newuser"));
    userID = userService.GetUserByName("newuser")?.ID ?? 0;
    Console.WriteLine("[ INFO  ] New user ID: {0}", userID);
    Console.WriteLine("[ TEST2 ] OK");
}
catch(Exception ex)
{
    Console.WriteLine("[ ERROR ] {0}", ex.GetType());
    Console.WriteLine("[ TEST2 ] FAILED");
}

Console.WriteLine("[ TEST3 ] Login");

User curUser = new User("", "");

try
{
    userService.Login(new LoginRequest("newuser", "newuser"));
    curUser = userService.GetCurrentUser();
    Console.WriteLine("[ INFO  ] Cur user Name: {0}", curUser.Name);
    
    if (curUser.Name == "newuser")
    {
        Console.WriteLine("[ TEST3 ] OK");
    }
    else
    {
        Console.WriteLine("[ ERROR ] Not newuser :(");
    }
}
catch(Exception ex)
{
    Console.WriteLine("[ ERROR ] {0}", ex.GetType());
    Console.WriteLine("[ TEST3 ] FAILED");
}

Console.WriteLine("[ TEST4 ] Event Registration");

var firstBGevent = boardGameEventRepository.GetByRegistration()[0];
try
{
    playerService.RegisterCurrentPlayerForEvent(firstBGevent);

    if (playerService.CheckCurrentPlayerRegistration(firstBGevent))
    {
        Console.WriteLine("[ TEST4 ] OK");
    }
    else
    {
        Console.WriteLine("[ ERROR ] There is not register");
    }
}
catch(Exception ex)
{
    Console.WriteLine("[ ERROR ] {0}", ex.GetType());
    Console.WriteLine("[ TEST4 ] FAILED");
}

Console.WriteLine("[ TEST5 ] Event Unregistration");

try
{
    playerService.UnregisterCurrentPlayerForEvent(firstBGevent);

    if (!playerService.CheckCurrentPlayerRegistration(firstBGevent))
    {
        Console.WriteLine("[ TEST5 ] OK");
    }
    else
    {
        Console.WriteLine("[ ERROR ] There is register");
    }
}
catch(Exception ex)
{
    Console.WriteLine("[ ERROR ] {0}", ex.GetType());
    Console.WriteLine("[ TEST5 ] FAILED");
}


Console.WriteLine("[ TEST5 ] Add to favorite");

try
{
    var game = boardGameService.GetBoardGameByID(1);
    boardGameService.AddBoardGameToFavorite(game);

    if (boardGameService.CheckBoardGameInCurrentUserFavorites(game))
    {
        Console.WriteLine("[ TEST5 ] OK");
    }
    else
    {
        Console.WriteLine("[ ERROR ] There is not in favorites");
    }
}
catch(Exception ex)
{
    Console.WriteLine("[ ERROR ] {0}", ex.GetType());
    Console.WriteLine("[ TEST5 ] FAILED");
}

Console.WriteLine("[ TEST6 ] Delete from favorite");

try
{
    var game = boardGameService.GetBoardGameByID(1);
    boardGameService.DeleteBoardGameFromFavorite(game);

    if (!boardGameService.CheckBoardGameInCurrentUserFavorites(game))
    {
        Console.WriteLine("[ TEST6 ] OK");
    }
    else
    {
        Console.WriteLine("[ ERROR ] There is in favorites");
    }
}
catch(Exception ex)
{
    Console.WriteLine("[ ERROR ] {0}", ex.GetType());
    Console.WriteLine("[ TEST6 ] FAILED");
}

Console.WriteLine("[ TEST7 ] Organizer Registration");

long orgID = 1006;

try
{
    orgID = organizerService.CreateOrganizer(new Organizer("ИП newuser", "Moscow"));
    Console.WriteLine("[ TEST7 ] OK");
}
catch(Exception ex)
{
    Console.WriteLine("[ ERROR ] {0}", ex.GetType());
    Console.WriteLine("[ TEST7 ] FAILED");
}

Console.WriteLine("[ TEST8 ] Delete Organizer");

try
{
    organizerService.DeleteOrganizer(organizerService.GetOrganizerByID(orgID));
    Console.WriteLine("[ TEST8 ] OK");
}
catch(Exception ex)
{
    Console.WriteLine("[ ERROR ] {0}", ex.GetType());
    Console.WriteLine("[ TEST8 ] FAILED");
}

Console.WriteLine("[ TEST9 ] Delete player");

var curPlayer = playerService.GetPlayerByName("newuser");
try
{
    playerService.DeletePlayer(curPlayer);
    if (playerService.GetPlayerByID(curPlayer.ID)?.Deleted == true)
    {
        Console.WriteLine("[ TEST9 ] OK");
    }
    else
    {
        Console.WriteLine("[ TEST9 ] FAILED ERROR");
    }
}
catch(Exception ex)
{
    Console.WriteLine("[ ERROR ] {0}", ex.GetType());
    Console.WriteLine("[ TEST9 ] FAILED");
}

context.Players.Remove(curPlayer);
context.SaveChanges();

Console.WriteLine("[ TEST10 ] Delete user");

try
{
    userService.DeleteUser(userService.GetUserByID(curUser.ID) ?? new User("", ""));
    Console.WriteLine("[ TEST10 ] OK");
}
catch(Exception ex)
{
    Console.WriteLine("[ ERROR ] {0}", ex.GetType());
    Console.WriteLine("[ TEST10 ] FAILED");
}

Console.WriteLine("FINISH");







