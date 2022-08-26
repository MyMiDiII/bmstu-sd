from bs4 import BeautifulSoup
import requests
import csv
import random


GAMES_SITE = "https://gaga.ru"
MAIN_URL = "/nastolnie-igri/"
SEP = "?"
PAGE_TMP = "p=%d"

DATADIR       = '..\\Data\\'
EXT           = '.csv'
GAMESFILE     = DATADIR + 'games' + EXT


def getGameName(soup):
    try:
        nameBox = soup.find('div', class_='card-btns')

        try:
            name = nameBox.find('button', class_='btn--oneclick btn')['data-name']
        except:
            name = nameBox.find('button', class_='btn add_to_waitlist')['data-name']
        
        return name
    except:
        return ""


def getGameProducer(soup):
    try:
        producerBox = soup.find('div', class_='card-btns')

        try:
            producer = producerBox.find('button', class_='btn--oneclick btn')['data-vendor']
        except:
            producer = producerBox.find('button', class_='btn add_to_waitlist')['data-vendor']
        
        return producer
    except:
        return ""


def getGameYear(soup):
    try:
        return random.randint(2008, 2022)
    except:
        return -1


def getAges(ageText):
    try:
        #minAge, maxAge = (int(ageText[:-1]), 150) if ageText[-1] == '+' else map(int, ageText.split()[0].split('-'))
        minAge = int(ageText.split()[1])
        maxAge = 150

        return minAge, maxAge
    except:
        return -1, -1


def getPlayers(players):
    try:
        players = players.split()
        minPlayers, maxPlayers = ([int(players[1])] * 2 if len(players) == 3
                                   else map(int, players[0].split('-')))

        return minPlayers, maxPlayers
    except:
        return -1, -1


def getDurations(durations):
    try:
        durations = [x.strip() for x in durations[:-3].split('-')]
        
        minDurations, maxDurations = ([int(float(durations[0]) * 60)] * 2
                                        if len(durations) == 1
                                        else [int(x * 60) for x in map(float, durations)])

        return minDurations, maxDurations
    except:
        return -1, -1


def loadGameInfo(soup):
    try:
        name = getGameName(soup)
        producer = getGameProducer(soup)
        year = getGameYear(soup)
        featuresList = [x.text for x in soup.find('ul', 'card-features__list').find_all('li')]
        minAge, maxAge = getAges(featuresList[2])
        minPlayers, maxPlayers = getPlayers(featuresList[1])
        minDuration, maxDuration = getDurations(featuresList[3])


        with open(GAMESFILE, "a", newline='') as games_file:
            writer = csv.writer(games_file, delimiter=',')

            writer.writerow(
                [
                    name,
                    producer,
                    year,
                    maxAge,
                    minAge,
                    maxPlayers,
                    minPlayers,
                    maxDuration,
                    minDuration,
                    False
                ]
            )
    except:
        pass


def loadGamesInfo(siteURL):
    pageCode = requests.get(siteURL, verify=False).text
    soup = BeautifulSoup(pageCode, 'lxml')

    lastPage = 67

    #with open(GAMESFILE, "w", newline='') as games_file:
    #    writer = csv.writer(games_file, delimiter=',')
    #    writer.writerow(
    #        [
    #            "Title",
    #            "Producer",
    #            "Year",    
    #            "MaxAge",  
    #            "MinAge",  
    #            "MaxPlayers",
    #            "MinPlayers",
    #            "MaxDuration",
    #            "MinDuration",
    #            "Deleted"
    #        ]
    #    )

    gamesNum = 973
    for i in range(55, lastPage + 1):
        curPageURL = (siteURL + MAIN_URL + SEP + PAGE_TMP) % i

        pageCode = requests.get(curPageURL, verify=False).text
        soup = BeautifulSoup(pageCode, 'lxml')

        gamesBoxes = soup.find_all('p', class_='preview-card__title')
        gamesRef = [x.find('a', href=True) for x in gamesBoxes]

        for gameRef in gamesRef:
            print(gamesNum, ".")

            ref = siteURL + gameRef['href']

            pageCode = requests.get(ref, verify=False).text
            soup = BeautifulSoup(pageCode, 'lxml')

            loadGameInfo(soup)

            gamesNum += 1


def generate_boardgames():
    loadGamesInfo(GAMES_SITE)


if __name__ == "__main__":
    generate_boardgames()