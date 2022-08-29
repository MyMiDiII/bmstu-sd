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

        writer.writerow(
            [
                "admin",
                2,
                0
            ]
        )

        usersIDs = list(range(3, 2003))
        random.shuffle(usersIDs)

        for i in range(PlayersGen.ROWNUM):

            writer.writerow(
                [
                    "player",
                    i + 1,
                    usersIDs[i]
                ]
            )

        orgUsersIDs = random.sample(usersIDs, OrganizersGen.ROWNUM)

        for i in range(OrganizersGen.ROWNUM):

            writer.writerow(
                [
                    "organizer",
                    i + 1,
                    orgUsersIDs[i]
                ]
            )


if __name__ == "__main__":
    generate_roles("en");
