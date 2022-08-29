import BoardGamesGen as BGG
import OrganizersGen as OG
import VenuesGen as VG
import BGEventsGen as BGEG

def generate_data():
    #BGG.generate_boardgames();
    #OG.generate_organizers("ru");
    #VG.generate_venues("ru");
    BGEG.generate_events("ru");

if __name__ == "__main__":
    generate_data();
