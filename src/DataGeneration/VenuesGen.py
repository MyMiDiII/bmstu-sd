import csv
import random

from faker import Faker
from faker_e164.providers import E164Provider

DATADIR       = '..\\Data\\'
EXT           = '.csv'
VENFILE       = DATADIR + 'venues' + EXT

ROWNUM = 2000
TYPES = [
    "кафе",
    "антикафе",
    "фастфуд",
    "ресторан",
    "магазин",
    "студия",
    "другое"
    ]

def fakerGenVenues(faker):
    with open(VENFILE, "w", newline='') as file:
        writer = csv.writer(file, delimiter=',')

        for i in range(ROWNUM):

            writer.writerow(
                [
                    faker.sentence(nb_words=2)[:-1],
                    random.choice(TYPES),
                    ','.join(faker.address().split(' ', 1)[-1].split(',')[:-1]),
                    faker.company_email(),
                    faker.url(),
                    faker.e164(region_code="RU"),
                    False
                ]
            )


def generate_venues(locale):
    if locale == "ru":
        faker = Faker("ru_RU")
    else:
        faker = Faker()

    faker.add_provider(E164Provider)

    fakerGenVenues(faker)


if __name__ == "__main__":
    generate_venues("ru");
