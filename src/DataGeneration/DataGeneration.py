import BoardGamesGen as BGG
import OrganizersGen as OG
import VenuesGen as VG

def generate_data():
    #BGG.generate_boardgames();
    OG.generate_organizers("ru");
    VG.generate_venues("ru");

if __name__ == "__main__":
    generate_data();
