import csv
import random

from faker import Faker

DATADIR       = '..\\Data\\'
EXT           = '.csv'
PLAYERFILE       = DATADIR + 'players' + EXT

ROWNUM = 2000
LEAGUES = [
    "Просто зашел",
    "Новичок",
    "Любитель",
    "Бывалый",
    "Опытный",
    "Знаток",
    "Профессионал",
    "Легенда"
    ]


def fakerGenPlayers(faker):
    with open(PLAYERFILE, "w", newline='') as file:
        writer = csv.writer(file, delimiter=',')

        for _ in range(ROWNUM):

            writer.writerow(
                [
                    faker.user_name(),
                    random.choice(LEAGUES),
                    random.randint(0, 100),
                    False
                ]
            )


def generate_players(locale):
    if locale == "ru":
        faker = Faker("ru_RU")
    else:
        faker = Faker()

    fakerGenPlayers(faker)


if __name__ == "__main__":
    generate_players("en");
