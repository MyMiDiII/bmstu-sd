truncate table "Games" restart identity cascade;

\copy "Games"("Title", "Producer", "Year", "MaxAge", "MinAge", "MaxPlayerNum",
			  "MinPlayerNum", "MaxDuration", "MinDuration", "Deleted")
from 'C:\Users\maslo\source\repos\software-design\src\Data\games.csv'
delimiter ',' csv header;