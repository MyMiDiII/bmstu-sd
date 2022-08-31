import csv
from passlib.hash import bcrypt

from faker import Faker

DATADIR       = '..\\Data\\'
EXT           = '.csv'
PLAYERFILE       = DATADIR + 'users' + EXT

ROWNUM = 2000


def fakerGenUsers(faker):
    with (open(PLAYERFILE, "w", newline='') as file,
          open("passwords.csv", "w", newline='') as addfile):
        writer = csv.writer(file, delimiter=',')
        addwriter = csv.writer(addfile, delimiter=',')
        writer.writerow(['guest', bcrypt.using(ident='2a').hash('guest'.encode('utf8'))])
        writer.writerow(['admin', bcrypt.using(ident='2a').hash('admin'.encode('utf8'))])
        writer.writerow(['player', bcrypt.using(ident='2a').hash('player'.encode('utf8'))])
        writer.writerow(['organizer', bcrypt.using(ident='2a').hash('organizer'.encode('utf8'))])
        addwriter.writerow(['guest', 'guest'])
        addwriter.writerow(['admin', 'admin'])
        addwriter.writerow(['player', 'player'])
        addwriter.writerow(['organizer', 'organizer'])

        for i in range(ROWNUM):
            print(i)
            name = faker.user_name()
            password = faker.password()

            addwriter.writerow([name, password])
            writer.writerow(
                [
                    name,
                    bcrypt.using(ident='2a').hash(password.encode('utf8'))
                ]
            )


def generate_users(locale):
    if locale == "ru":
        faker = Faker("ru_RU")
    else:
        faker = Faker()

    fakerGenUsers(faker)


if __name__ == "__main__":
    generate_users("en");




