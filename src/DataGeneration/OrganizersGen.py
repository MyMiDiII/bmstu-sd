import csv

from faker import Faker
from faker_e164.providers import E164Provider

DATADIR       = '..\\Data\\'
EXT           = '.csv'
ORGFILE       = DATADIR + 'organizers' + EXT

ROWNUM = 1000

def fakerGenOrganizers(faker):
    with open(ORGFILE, "w", newline='') as file:
        writer = csv.writer(file, delimiter=',')

        for i in range(ROWNUM):

            writer.writerow(
                [
                    faker.company(),
                    ','.join(faker.address().split(' ', 1)[-1].split(',')[:-1]),
                    faker.company_email(),
                    faker.url(),
                    faker.e164(region_code="RU"),
                    False
                ]
            )


def generate_organizers(locale):
    if locale == "ru":
        faker = Faker("ru_RU")
    else:
        faker = Faker()

    faker.add_provider(E164Provider)

    fakerGenOrganizers(faker)


if __name__ == "__main__":
    generate_organizers("ru");

