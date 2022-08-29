import csv
import bcrypt

from faker import Faker

DATADIR       = '..\\Data\\'
EXT           = '.csv'
PLAYERFILE       = DATADIR + 'users' + EXT

ROWNUM = 2000


def fakerGenUsers(faker):
    with open(PLAYERFILE, "a", newline='') as file:
        writer = csv.writer(file, delimiter=',')
        writer.writerow(['admin', bcrypt.hashpw('admin'.encode('utf8'), bcrypt.gensalt())])

        #for _ in range(ROWNUM):

        #    writer.writerow(
        #        [
        #            faker.user_name(),
        #            bcrypt.hashpw(faker.password().encode('utf8'), bcrypt.gensalt()),
        #        ]
        #    )


def generate_users(locale):
    if locale == "ru":
        faker = Faker("ru_RU")
    else:
        faker = Faker()

    fakerGenUsers(faker)


if __name__ == "__main__":
    generate_users("en");




