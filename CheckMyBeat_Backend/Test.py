from Database import *
db = Database(Parameters.port, Parameters.databaseName)
db.empty()
db.initialize()
while(True):
    string = input("Text")
    print(db.checkUsernameAvailability(string))
print(db.runQuery({}, "user"))