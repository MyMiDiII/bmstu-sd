import csv
import random

import PlayersGen
import OrganizersGen

DATADIR       = '..\\Data\\'
EXT           = '.csv'
ROLEFILE       = DATADIR + 'roles' + EXT


def generate_roles():
    with open(ROLEFILE, "w", newline='') as file:
        writer = csv.writer(file, delimiter=',')

        writer.writerow( [ "guest", 0, 1 ] )
        writer.writerow( [ "admin", 0, 2 ] )
        writer.writerow( [ "player", 1, 3 ] )
        writer.writerow( [ "player", 2, 4 ] )
        writer.writerow( [ "orginizer", 1, 4 ] )

        usersIDs = list(range(5, 2005))
        random.shuffle(usersIDs)

        for i in range(3, PlayersGen.ROWNUM):

            writer.writerow(
                [
                    "player",
                    i + 1,
                    usersIDs[i]
                ]
            )

        orgUsersIDs = random.sample(usersIDs, OrganizersGen.ROWNUM)

        for i in range(2, OrganizersGen.ROWNUM):

            writer.writerow(
                [
                    "organizer",
                    i + 1,
                    orgUsersIDs[i]
                ]
            )


if __name__ == "__main__":
    generate_roles("en");
