import BoardGamesGen as BGG
import OrganizersGen as OG
import VenuesGen as VG
import BGEventsGen as BGEG
import PlayersGen as PG
import UsersGen as UG
import RolesGen as RG
import RelationsGen as RLG


def generate_data():
    #BGG.generate_boardgames()
    #OG.generate_organizers("ru")
    #VG.generate_venues("ru")
    BGEG.generate_events("ru")
    #PG.generate_players("en")
    #UG.generate_users("en")
    #RG.generate_roles()
    #RLG.generate_relations()


if __name__ == "__main__":
    generate_data();
