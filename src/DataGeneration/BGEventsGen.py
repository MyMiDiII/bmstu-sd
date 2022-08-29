import csv
import random

from faker import Faker
from datetime import datetime, timedelta
from enum import IntEnum, auto

import OrganizersGen
import VenuesGen

DATADIR       = '..\\Data\\'
EXT           = '.csv'
BGEFILE       = DATADIR + 'events' + EXT

ROWNUM = 1000

class BGEstate(IntEnum):
    Planned = auto()
    Registration = auto()
    Ready = auto()
    Started = auto()
    Finished = auto()
    Cancelled = auto()
    Deleted = auto()


def fakerGenEvents(faker):
    with open(BGEFILE, "w", newline='') as file:
        writer = csv.writer(file, delimiter=',')

        for _ in range(ROWNUM):
            eventDateTime = faker.date_time_between(
                                datetime(2021, 1, 1, 0, 0, 0),
                                datetime(2022, 9, 30, 23, 59, 59)).replace(second=0)
            duration = random.randint(3, 144) * 5
            beginRegDateTime = faker.date_time_between(eventDateTime - timedelta(days=30),
                                               eventDateTime - timedelta(minutes=30)).replace(second=0)
            endRegDateTime = faker.date_time_between(beginRegDateTime, eventDateTime).replace(second=0)
            curDateTime = datetime.now().replace(second=0)
            cancelled = random.randint(0, 100) > 95
            state = int(BGEstate.Cancelled if cancelled
                    else (BGEstate.Planned if curDateTime < beginRegDateTime
                    else (BGEstate.Registration if beginRegDateTime <= curDateTime < endRegDateTime
                    else (BGEstate.Ready if endRegDateTime <= curDateTime < eventDateTime
                    else (BGEstate.Started
                          if  eventDateTime <= curDateTime < eventDateTime + timedelta(minutes=duration)
                    else BGEstate.Finished)))))

            writer.writerow(
                [
                    faker.sentence(nb_words=3)[:-1],
                    eventDateTime.date(),
                    eventDateTime.time(),
                    random.randint(3, 144) * 5,
                    random.randint(0, 1000),
                    bool(random.randint(0, 1)),
                    beginRegDateTime,
                    endRegDateTime,
                    state,
                    random.randint(1, OrganizersGen.ROWNUM),
                    random.randint(1, VenuesGen.ROWNUM),
                    False
                ]
            )


def generate_events(locale):
    if locale == "ru":
        faker = Faker("ru_RU")
    else:
        faker = Faker()

    fakerGenEvents(faker)


if __name__ == "__main__":
    generate_events("ru");
