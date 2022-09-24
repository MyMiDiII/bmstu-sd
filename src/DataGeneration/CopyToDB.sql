truncate table "Games" restart identity cascade;

\copy "Games"("Title", "Producer", "Year", "MaxAge", "MinAge", "MaxPlayerNum",
			  "MinPlayerNum", "MaxDuration", "MinDuration", "Deleted")
from 'C:\Users\maslo\source\repos\software-design\src\Data\games.csv'
delimiter ',' csv header;

\copy "Organizers"("Name", "Address", "Email", "URL", "PhoneNumber", "Deleted")
from 'C:\Users\maslo\source\repos\software-design\src\Data\organizers.csv'
delimiter ',' csv;

\copy "Players"("Name", "League", "Rating", "Deleted")
from 'C:\Users\maslo\source\repos\software-design\src\Data\players.csv'
delimiter ',' csv;

\copy "Users"("Name", "Password")
from 'C:\Users\maslo\source\repos\software-design\src\Data\users.csv'
delimiter ',' csv;

\copy "Venues"("Name", "Type", "Address", "Email", "URL", "PhoneNumber", "Deleted")
from 'C:\Users\maslo\source\repos\software-design\src\Data\venues.csv'
delimiter ',' csv;

\copy "Events"("Title", "Date", "StartTime", "Duration", "Cost", "Purchase",
			   "BeginRegistration", "EndRegistration", "State", "OrganizerID",
			   "VenueID", "Deleted")
from 'C:\Users\maslo\source\repos\software-design\src\Data\events.csv'
delimiter ',' csv;

\copy "Favorites"("BoardGameID", "PlayerID")
from 'C:\Users\maslo\source\repos\software-design\src\Data\favorites.csv'
delimiter ',' csv;

\copy "EventGameRelations"("BoardGameID", "BoardGameEventID")
from 'C:\Users\maslo\source\repos\software-design\src\Data\gameevent.csv'
delimiter ',' csv;

\copy "Registrations"("PlayerID", "BoardGameEventID")
from 'C:\Users\maslo\source\repos\software-design\src\Data\registrations.csv'
delimiter ',' csv;

\copy "Roles"("RoleName", "RoleID", "UserID")
from 'C:\Users\maslo\source\repos\software-design\src\Data\roles.csv'
delimiter ',' csv;
