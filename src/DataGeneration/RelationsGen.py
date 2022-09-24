import csv
import random

import BGEventsGen
import PlayersGen

DATADIR       = '..\\Data\\'
EXT           = '.csv'
GAMEEVENTFILE = DATADIR + 'gameevent' + EXT
FAVORITESFILE = DATADIR + 'favorites' + EXT
REGSFILE      = DATADIR + 'registrations' + EXT

MAXGAMEID = 1171
GAMESIDs = list(range(1, 1172))
PLAYERSIDs = list(range(1, PlayersGen.ROWNUM + 1))

def generate_gameevent():
    with open(GAMEEVENTFILE, "w", newline='') as file:
        writer = csv.writer(file, delimiter=',')

        for i in range(BGEventsGen.ROWNUM):
            gamesNum = random.randint(1, 10)
            curEventGamesIDs = random.sample(GAMESIDs, gamesNum)

            for gameID in curEventGamesIDs:
                writer.writerow(
                    [
                        gameID,
                        i + 1
                    ]
                )


def generate_favorites():
    with open(FAVORITESFILE, "w", newline='') as file:
        writer = csv.writer(file, delimiter=',')

        for i in range(PlayersGen.ROWNUM):
            gamesNum = random.randint(0, 20)
            curPlayerGamesIDs = random.sample(GAMESIDs, gamesNum)

            for gameID in curPlayerGamesIDs:
                writer.writerow(
                    [
                        gameID,
                        i + 1
                    ]
                )


def generate_registrations():
    with open(REGSFILE, "w", newline='') as file:
        writer = csv.writer(file, delimiter=',')

        for i in range(BGEventsGen.ROWNUM):
            playersNum = random.randint(1, 50)
            curEventPlayersIDs = random.sample(PLAYERSIDs, playersNum)

            for playerID in curEventPlayersIDs:
                writer.writerow(
                    [
                        playerID,
                        i + 1
                    ]
                )


def generate_relations():
    generate_gameevent()
    generate_favorites()
    generate_registrations()


if __name__ == "__main__":
    generate_relations();
