from pymongo import MongoClient
from bson.objectid import ObjectId
from datetime import datetime
import re
import pandas as pd
import plotly.express as px
import numpy as np
import os
import plotly
from plotly.offline import iplot, init_notebook_mode
import plotly.graph_objects as go
import base64
from datetime import date, datetime

plotly.io.orca.config.executable = "C:\\Users\\dell\\AppData\\Local\\Programs\\orca\\orca.exe"

class Parameters():
    port = 27017
    databaseName = "LRC"
    
    success ={"status":"success"}
    failure ={"status":"failure"}

    availableUsername= {"status": "Valid Username." }
    unavailableUsername ={"status": "Invalid Username." }
    
    regexEmail = '^[a-z0-9]+[\._]?[a-z0-9]+[@]\w+([.]\w{2,3}){1,3}$'
    invalidEmailAddress ={"status": "Invalid email address."}
    unavailableEmailAddress ={"status": "Invalid email address."}
    validEmailAddress = {"status": "Valid email address."}
    
    authenticationRequestNotValidated ={"status":"Credential request was not checked yet.","reason": "", "username":"","password":"","email":"","name":"","dateOfBirth":"" , "gender":"","picture":"", "role":"","roleDescription":""}
    authenticationRequestNotApproved = {"status":"Credential request was not approved.","reason": "", "username":"","password":"","email":"","name":"","dateOfBirth":"" , "gender":"","picture":"" ,"role":"","roleDescription":""}
    authenticationFailed = {"status":"Invalid username and/or password","reason": "", "username":"","password":"","email":"","name":"","dateOfBirth":"" , "gender":"","picture":"" ,"role":"","roleDescription":""}
    @staticmethod
    def formatAuthenticationSuccessful(userData, message=""):
        return {"status": message+" Successful Login" , "reason": userData["reason"], "username":userData["username"],"password":userData["password"],"email":userData["email"],"name":userData["name"],"dateOfBirth":userData["dateOfBirth"] , "gender":userData["gender"],"picture":userData["picture"], "role":userData["role"],"roleDescription":userData["roleDescription"]}
    @staticmethod
    def formatAuthenticationFailure(message=""):
        return {"status": " Credential request was not approved. Reason: "+message , "reason": "", "username":"","password":"","email":"","name":"","dateOfBirth":"" , "gender":"","picture":"", "role":"","roleDescription":""}
    
    NotApproved ="Credential request was not approved."
    Approved ="Credential request was approved. Kindly Login."
    NotValidated= "Credential request was not checked yet."
    NotPresent ="Not Present"
    userAlreadyExists ="User already exist. Kindly change the username."
    
    
    requestSent  ={"status":"Your request has been sent"}
    @staticmethod
    def formatRequestNotSent( message):
        return {"status": message+" Your request has not been sent." }
 
class Database():
    def __init__(self, port, databaseName):
        self.client = MongoClient(port=port)
        self.database = self.client[databaseName]
        
        self.accountRequests = self.database.accountRequests # username*, password*, email*, name*, gender, dateOfBirth, role*, roleDescription*, picture
        self.users = self.database.users # username*, password*, email*, name*, gender, dateOfBirth, role*, roleDescription*, picture
        self.roleChangeRequests = self.database.roleChangeRequests #username*, newRole*, dateOfRequest*, reason*, response where the response is null

        self.patients = self.database.patients #patientID*, name*, gender*, picture*, bloodType, diseases

        self.ecgFetched = self.database.ecgFetched #ecgID*, dataPoints*, picture*, dateOfAcquisition*, timeOfAcquisition*
        self.ecgRecords = self.database.ecgRecords # ecgID*, patientID*, dataPoints*, featuresExtracted*, dateOfAcquisition*, timeOfAcquisition*, nameOfUser*, machineDiagnosis*, humanDiagnosis
        
        self.history = self.database.history
        self.logindates=self.database.logindates #dates 
        self.statistics= self.database.statistics #number, title, image_bytes 

    def empty(self):
        self.database.statistics.delete_many({})
        self.database.logindates.delete_many({})
        self.database.accountRequests.delete_many({})
        self.database.users.delete_many({})
        self.database.roleChangeRequests.delete_many({})
        self.database.patients.delete_many({})
        self.database.ecgFetched.delete_many({})
        self.database.ecgRecords.delete_many({})
        
    def toList(self,request):
        """Return results as a list"""
        return [] if request is None else list(request)
    
    def initialize(self):
        self.users.insert_one({"reason":"","status":"Allowed","username":"admin", "password": "admin123","email":"admin@email.com", "name":"system admin","isAuthenticated": "yes", "dateOfBirth": "12/06/2020", "gender":"Other", "picture": "", "role": "administrator", "roleDescription":"The system administrator " })
        self.patients.insert_one({"patientId":"P0001", "gender":"MALE",'name': 'Dummy Patient', 'picture': '', 'bloodType': 'A+', 'diseases': ['NO RISKS', 'NO RISK']})
        self.users.insert_one({"reason":"","status":"Allowed","username":"clara", "password": "clara123","email":"claraakiki@email.com", "name":"Clara Akiki","isAuthenticated": "yes", "dateOfBirth": "29/09/1997", "gender":"", "picture": "", "role": "operator", "roleDescription":"The operator is able to add a patient, search for a patient, update the records of this patient. He/she is able to retrieve the ECG signals, select an ECG signal to request its diagnosis and attribute both diagnosis and ECG to a certain patient. This user can update his/her profile and access statistics." })
        self.users.insert_one({"reason":"","status":"Allowed","username":"joseph", "password": "joseph123","email":"josephattieh@email.com", "name":"Joseph Attieh","isAuthenticated": "yes", "dateOfBirth": "31/10/1998", "gender":"", "picture": "", "role": "operator", "roleDescription":"The operator is able to add a patient, search for a patient, update the records of this patient. He/she is able to retrieve the ECG signals, select an ECG signal to request its diagnosis and attribute both diagnosis and ECG to a certain patient. This user can update his/her profile and access statistics." })
        self.users.insert_one({"reason":"","status":"Allowed","username":"guy", "password": "guy123","email":"guyabihanna@email.com", "name":"Guy Abi Hanna","isAuthenticated": "yes", "dateOfBirth": "17/08/1997", "gender":"", "picture": "", "role": "operator", "roleDescription":"The operator is able to add a patient, search for a patient, update the records of this patient. He/she is able to retrieve the ECG signals, select an ECG signal to request its diagnosis and attribute both diagnosis and ECG to a certain patient. This user can update his/her profile and access statistics." })
        self.users.insert_one({"reason":"","status":"Allowed","username":"grace", "password": "grace123","email":"graceakiki@email.com", "name":"Grace Akiki","isAuthenticated": "yes", "dateOfBirth": "17/10/1996", "gender":"", "picture": "", "role": "professional", "roleDescription":"" })
        self.users.insert_one({"reason":"","status":"Allowed","username":"joe", "password": "joe123","email":"joetekli@email.com", "name":"Joe Tekli","isAuthenticated": "yes", "dateOfBirth": "17/10/1980", "gender":"", "picture": "", "role": "professional", "roleDescription":"" })
        self.accountRequests.insert_one({"reason":"","status":"","username":"josephat", "password": "admin123","email":"ajauin@email.com", "name":"joseph attieh","isAuthenticated": "yes", "dateOfBirth": "12/06/2020", "gender":"", "picture": "", "role": "operator", "roleDescription":"The system administrator have access to the same functionalities available to the ECG operator. Additionally, he/she is able to search for the users in the system and update their user role if needed. Furthermore, he/she will be able to approve the account requests of the other users. " })
        self.accountRequests.insert_one({"reason":"","status":"Approved","username":"claraak", "password": "admin123","email":"ajauin@email.com", "name":"clara akiki","isAuthenticated": "yes", "dateOfBirth": "12/06/2020", "gender":"", "picture": "", "role": "professional", "roleDescription":"The system administrator have access to the same functionalities available to the ECG operator. Additionally, he/she is able to search for the users in the system and update their user role if needed. Furthermore, he/she will be able to approve the account requests of the other users. " })
        self.accountRequests.insert_one({"reason":"Not allowed !","status":"Not Approved","username":"guy", "password": "admin123","email":"ajauin@email.com", "name":"guy abi hanna","isAuthenticated": "yes", "dateOfBirth": "12/06/2020", "gender":"", "picture": "", "role": "administrator", "roleDescription":"The system administrator have access to the same functionalities available to the ECG operator. Additionally, he/she is able to search for the users in the system and update their user role if needed. Furthermore, he/she will be able to approve the account requests of the other users. " })
        self.patients.insert_one({"patientId":"P0000", "name":"John Doe", "gender":"MALE", "picture":"", "bloodType":"A+", "diseases":["Obesity", "Smoker"]})
        self.patients.insert_one({"patientId":"P0002", "name":"Jane Doe", "gender":"FEMALE", "picture":"", "bloodType":"O+", "diseases":["Diabetes"]})
        self.patients.insert_one({"patientId":"P0003", "name":"Johny Doe", "gender":"MALE", "picture":"", "bloodType":"O+", "diseases":["High Blood Pressure", "High Cholesterol"]})
        self.patients.insert_one({"patientId":"P0004", "name":"Janet Doe", "gender":"FEMALE", "picture":"", "bloodType":"B+", "diseases":["High Blood Pressure", "Obesity"]})
        self.patients.insert_one({"patientId":"P0005", "name":"Johnathan Doe", "gender":"MALE", "picture":"", "bloodType":"O+", "diseases":["Physical Inactivity", "High Cholesterol"]})
        self.patients.insert_one({"patientId":"P0006", "name":"John Snow", "gender":"MALE", "picture":"", "bloodType":"A+", "diseases":["Diabetes", "Physical Inactivity"]})
        
        self.logindates.insert_one({"dates": ['29/05/2020','29/05/2020','30/05/2020','30/05/2020','30/05/2020','31/05/2020', '31/05/2020', '31/05/2020', '31/05/2020', '31/05/2020', '31/05/2020', '31/05/2020']})
        
        self.statistics.insert_one({"number": 1,"title": "Patient Gender Distribution" ,"image_bytes": ""})
        self.statistics.insert_one({"number": 2,"title": "Patient Risk Factor Distribution" ,"image_bytes": ""})
        self.statistics.insert_one({"number": 3,"title": "ECG Abnormalities Distribution" ,"image_bytes": ""})
        self.statistics.insert_one({"number": 4,"title": "Machine VS Human Diagnosis" ,"image_bytes": ""})
        self.statistics.insert_one({"number": 5,"title": "User Role Distribution" ,"image_bytes": ""})
        self.statistics.insert_one({"number": 6,"title": "User Activity" ,"image_bytes": ""})


    #users DB functionalities
    
    #username*, password*, email*, name*, gender, dateOfBirth, role*, roleDescription*, picture
    def checkUsernameAvailability(self, username):        
        response = self.toList(self.users.find({"username": username})) + self.toList(self.accountRequests.find({"username": username}))
        return  Parameters.availableUsername if len(response) ==0 else Parameters.unavailableUsername
    
    def checkEmailAvailability(self, email):
        if not re.search(Parameters.regexEmail,email) :
            return Parameters.invalidEmailAddress
        response = self.toList(self.users.find({"email": email})) + self.toList(self.accountRequests.find({"email": email}))
        if len(response)!=0:
            return Parameters.unavailableEmailAddress
        return  Parameters.validEmailAddress

    def populatePatientLessEcgs(self):
        import json.decoder
        with open('ECGsOld.json') as json_file:
            data = json.load(json_file)
        for record in data:
            self.addPatientlessECG(record["ecgId"], record["data"], record["fs"], record["picture"], record["numberOfRRIntervals"])
        return Parameters.success
    #authentication
    def authenticate(self, username, password):
        response= self.toList(self.users.find({"username": username, "password" : password  }))
        if len(response)==0:
            resp = self.isNewRequestAccount(username)
            if resp == Parameters.Approved: 
                #here we move the record to the (users) database since he was approved
                response = self.toList(self.accountRequests.find({"username": username}))
                if response[0]["password"] !=password:
                    return Parameters.authenticationFailed
                self.users.insert_one(response[0])
                self.accountRequests.delete_many({"username": username, "password" : password  })
                req = self.toList(self.roleChangeRequests.find({"username":username}))
                message =""
                if len(req)!=0:
                    if req[0]["status"] !="":
                        message = "Role change request was "+req[0]["status"]  +" by "+req[0]["by"]+" ."
                        if req[0]["reason"]!="":
                            message += " : \" "+req[0]["reason"] +"\"." 
                        #delete request
                        self.roleChangeRequests.delete_many({"username":username})
                        print("deleting")
                    else:
                        message = "Role change request was not processed yet."
                return Parameters.formatAuthenticationSuccessful(response[0], "Request was granted! "+message)
            elif resp == Parameters.NotApproved: 
                response = self.toList(self.accountRequests.find({"username": username}))
                if response[0]["password"] !=password:
                    return Parameters.authenticationFailed
                return Parameters.formatAuthenticationFailure(  response[0]["reason"])
            elif resp == Parameters.NotValidated: 
                return Parameters.authenticationRequestNotValidated
            else:
                return Parameters.authenticationFailed

        req = self.toList(self.roleChangeRequests.find({"username":username}))
        message =""
        if len(req)!=0:
            if req[0]["status"] !="":
                message = "Role change request was "+req[0]["status"]  +" by "+req[0]["by"]+" ."
                if req[0]["reason"]!="":
                         message += " : \" "+req[0]["reason"] +"\"." 
                        #delete request
                self.roleChangeRequests.delete_many({"username":username})
                print("deleting")
            else:
                message = "Role change request was not processed yet."
    
        oldDates=self.logindates.find_one()
        self.logindates.update_one({"_id": oldDates["_id"]},{ "$set": { "dates": oldDates["dates"]+ [date.today().strftime("%d/%m/%Y")] }})

        return Parameters.formatAuthenticationSuccessful(response[0], "Request was granted! "+message)
    
    """ here the user is not allowed to request if the username and emails were not successfully validated by the methods above
        This is called when adding the user to (accountRequests)"""
    def isNewRequestAccount(self,username):
        if len(self.toList(self.users.find({"username": username})))!=0:
            return Parameters.userAlreadyExists 
        response = self.toList(self.accountRequests.find({"username": username}))
        if len(response)!=0:
            if(response[0]["status"] =="Not Approved"):
                    return Parameters.NotApproved
            elif response[0]["status"] =="Approved": 
                    return Parameters.Approved
            else: return Parameters.NotValidated
        else:
            return Parameters.NotPresent

    def requestAccount(self,username, password, email, name, gender, dateOfBirth, role, roleDescription, picture):
        userRequest={"reason":"", "status":"", "username":username, "password":password,"email": email, "name":name, "gender":gender, "dateOfBirth":dateOfBirth, "role":role, "roleDescription":roleDescription, "picture":picture}
        response = self.isNewRequestAccount(username)
        if response in [Parameters.NotPresent,Parameters.NotApproved ]:
            self.accountRequests.insert_one(userRequest)
            return Parameters.requestSent
        else:
            return Parameters.formatRequestNotSent(response)
        
    def approveRequest(self,username):
        self.accountRequests.update_many({"username": username, "status":""},{ "$set": { "status": "Approved" } })
        return Parameters.success
    
    def rejectRequest(self,username, reason):
        self.accountRequests.update_many({"username": username,"status":""},{ "$set": { "status": "Not Approved", "reason":reason} })
        return Parameters.success
    
    def fetchAccountCreationRequests(self):
        return self.toList(self.accountRequests.find({"status":""}))
    
    def fetchAccounts(self):
        return self.toList(self.users.find({}))
    
    def getUserAccount(self, username):
        result = self.toList(self.users.find({"username":username}))
        return result
        
    #could delete my own, or an admin could do that, needs only username
    def deleteUserAccount(self, username):
        self.users.delete_many({"username":username});
        self.roleChangeRequests.delete_many({"username":username});
        return Parameters.success
    
    #could update my own, or an admin could do that, needs  only username and cannot change role or username
    def updateUserAccount(self,username, password, email, name, gender, dateOfBirth, roleDescription, picture):
        userRequest={ "password":password,"email": email, "name":name, "gender":gender, "dateOfBirth":dateOfBirth,  "roleDescription":roleDescription, "picture":picture}
        self.users.update_many({"username": username},{ "$set": userRequest })
        return Parameters.success
        
    #username*, newRole*, dateOfRequest*, reason*, response where the response is null
    #role change request
    def requestRoleChange(self,username, newRole, reason):
        
        today = date.today().strftime("%d/%m/%Y")
        time = datetime.now().strftime("%H:%M:%S")
        roleChange = {"username": username, "newRole":newRole, "dateOfRequest":today, "timeOfRequest":time, "reason":reason}

        if self.checkUsernameAvailability(username) == Parameters.availableUsername:
            return Parameters.failure
        #check if same role
        if len(self.toList(self.users.find({"username":username, "role":newRole})))!=0:
            if len(self.toList(self.roleChangeRequests.find({"username":username})))==0:
                self.roleChangeRequests.update_many({"username": username},{ "$set": roleChange})
                return Parameters.success
            else:
                print(self.toList(self.users.find({"username":username, "role":newRole})))
                return Parameters.failure

        #if first request, add it ... else update the request
        if len(self.toList(self.roleChangeRequests.find({"username":username})))==0:
            roleChange["status"]=""
            roleChange["reasonRejected"]=""
            roleChange["by"]=""
            self.roleChangeRequests.insert_one(roleChange)
        else:
            #i do not affect a role if granted
            roleChange["status"]=""
            roleChange["reasonRejected"]=""
            roleChange["by"]=""
            self.roleChangeRequests.update_many({"username": username},{ "$set": roleChange})
        return Parameters.success
        
    def fetchRoleChangeRequests(self):
        return self.toList(self.roleChangeRequests.find({"status":""}))

    #myUsername is the username of the user who approved
    def approveRoleChangeRequest(self,username, myUsername):
        newRole = self.toList(self.roleChangeRequests.find({"username":username}))[0]["newRole"]
        self.users.update_many({"username": username},{ "$set": { "role": newRole } })
        self.roleChangeRequests.update_many({"username": username},{ "$set": { "status": "Approved" , "by":myUsername  } })
        return Parameters.success
    
    def rejectRoleChangeRequest(self,username, reasonRejected, myUsername):
        self.roleChangeRequests.update_many({"username": username},{ "$set": { "status": "Not Approved", "reasonRejected":reasonRejected , "by":myUsername } })
        return Parameters.success
    
    def fetchPatients(self):
        return self.toList(self.patients.find({}))
    
    def addPatient(self, patientId,name,gender,picture,bloodType,diseases):
        if len(self.getPatient(patientId))!=0:
            return Parameters.failure
        record ={"patientId":patientId, "name":name, "gender":gender, "picture":picture, "bloodType":bloodType, "diseases":diseases}
        self.patients.insert_one(record)
        return Parameters.success

    def getPatient(self, patientId):
        results= self.toList(self.patients.find({"patientId":patientId}))
        return results
    def deletePatient(self, patientId):
        self.patients.delete_many({"patientId":patientId})
        self.ecgRecords.delete_many({"patientId":patientId})
        return Parameters.success
    
    def updatePatient(self, patientId,name,gender,picture,bloodType,diseases):
        self.patients.update_many({"patientId":patientId},{"$set":{"name":name, "gender":gender, "picture":picture, "bloodType":bloodType, "diseases":diseases} })
        return Parameters.success

    #ecgID*, dataPoints*, picture*, dateOfAcquisition*, timeOfAcquisition*
    def fetchPatientlessECGs(self):
        return self.toList(self.ecgFetched.find({}))
    
    def addPatientlessECG(self, ecgId, dataPoints,fs, picture, numberOfRRIntervals):
        if len((self.toList(self.ecgFetched.find({"ecgId":ecgId}))))!=0:
            return Parameters.failure
        day = date.today().strftime("%d/%m/%Y")
        time = datetime.now().strftime("%H:%M:%S")
        self.ecgFetched.insert_one({"ecgId":ecgId,"data":dataPoints,"fs":fs, "dateOfAcquisition":day, "timeOfAcquisition": time, "picture":picture, "numberOfRRIntervals":numberOfRRIntervals})
        return Parameters.success

    def getPatientlessECG(self, ecgId):
        results= self.toList(self.ecgFetched.find({"ecgId":ecgId}))
        return results
    def removePatientlessECG(self, ecgId):
        self.ecgFetched.delete_many({"ecgId":ecgId})
        return Parameters.success
    
    #   featuresExtracted*, nameOfUser*, machineDiagnosis*, humanDiagnosis
    def fetchECGPatientRecords(self):
        return self.toList(self.ecgRecords.find({}))
    
    def getECGsByPatientId(self, patientId):
        return self.toList(self.ecgRecords.find({"patientId":patientId}))
    
    def getECGRecordByECGId(self, ecgId):
        results= self.toList(self.ecgRecords.find({"ecgId":ecgId}))
        return results

    def deleteECGfromPatientRecord(self, ecgId):
        self.ecgRecords.delete_many({"ecgId":ecgId})
        return Parameters.success

    def associateECGtoPatient(self, ecgId, patientId, myUsername):
        #delete from fetched and add patientId
        data =self.getPatientlessECG(ecgId)
        if len(data)==0:
            return Parameters.failure
        if len(self.getPatient(patientId))==0:
            return Parameters.failure
        self.removePatientlessECG(ecgId)
        data = data[0]
        data["bpm"]=""
        data["patientId"] = patientId
        data["user"] =myUsername
        data["pExtracted"] =[]
        data["qExtracted"]=[]
        data["rExtracted"]=[]
        data["sExtracted"]=[]
        data["tExtracted"]=[]
        data["machineDiagnosisKey"] =[]
        data["machineDiagnosisProba"] =[]
        data["machineDiagnosisBoolean"] =[]        
        self.ecgRecords.insert_one(data)
        return Parameters.success
    
    def updateECGData(self, ecgId, bpm,pExtracted,qExtracted,rExtracted,sExtracted,tExtracted, machineDiagnosisKey,machineDiagnosisProba,machineDiagnosisBoolean, humanDiagnosis ):
        data =self.getECGRecordByECGId(ecgId)
        if len(data)==0:
            return Parameters.failure
        data = data[0]
        self.ecgRecords.update_many({"ecgId":ecgId}, {"$set":{"bpm":bpm, "pExtracted": pExtracted, "qExtracted":qExtracted,"rExtracted":rExtracted,"sExtracted":sExtracted,"tExtracted":tExtracted, "machineDiagnosisKey":machineDiagnosisKey,"machineDiagnosisProba":machineDiagnosisProba,"machineDiagnosisBoolean":machineDiagnosisBoolean ,"humanDiagnosis":humanDiagnosis}})
        return Parameters.success


#SEARCH

    def searchWildCard(self, dictionary, field):
        results=[]
        list_keys = list(dictionary.keys())
        if len(list_keys)==0:
            return field.find({})
        newDict = self.transform(dictionary)
        print(newDict)
        return self.toList(field.find(newDict))

    def transform(self, dictionary):
        tR={}
        for k in dictionary:
            val = dictionary[k]
            if type(val) is list:
                tR[k] = re.compile("(.*"+".*|.*".join(val)+".*)", re.IGNORECASE)
            else:
                tR[k] = re.compile(".*"+(val)+".*", re.IGNORECASE) 
        return tR
    #search patients: #patientID*, name*, gender*, bloodType, diseases
    #search users: username*, email*, name*, gender, dateOfBirth, role*
    def search(self, dictionary, field):
        
        results=[]
        list_keys = list(dictionary.keys())
        if len(list_keys)==0:
            return field.find({})
        for k, v in dictionary.items():
            if isinstance(v, list):
                for vv in v:
                    regex='^'+ vv
                    if (not results) and (list_keys.index(k) == 0):
                        results=self.toList(field.find({k: re.compile(regex, re.IGNORECASE)}))
                    else:
                        results1=self.toList(field.find({k: re.compile(regex, re.IGNORECASE)}))
                        results=[d for d in results if d in results1]
            elif isinstance(v, int):
                if (not results) and (list_keys.index(k) == 0):
                    results=self.toList(field.find({k: v}))
                    print(results)
                else:
                    results1=self.toList(field.find({k: v}))
                    results=[d for d in results if d in results1]
            else:
                regex='^'+ v
                if (not results) and (list_keys.index(k) == 0):
                    results=self.toList(field.find({k: re.compile(regex, re.IGNORECASE)}))
                    print(results)
                else:
                    results1=self.toList(field.find({k: re.compile(regex, re.IGNORECASE)}))
                    results=[d for d in results if d in results1]

        new_results = []
        for i in range(len(results)): 
            if results[i] not in results[i + 1:]: 
                new_results.append(results[i]) 
        return new_results



    #search user creation requests: username*, password*, email*, name*, gender, dateOfBirth, role*, roleDescription*
    #search user change role requests: username*, newRole*, dateOfRequest*, reason*
    def searchRequests(self, dictionary, field):
        results=[]
        list_keys = list(dictionary.keys())
        print(list_keys)
        for k, v in dictionary.items():
            if not isinstance(v, list):
                regex='^'+ v
                if (not results) and (list_keys.index(k) == 0):
                    results=self.toList(field.find({k: re.compile(regex, re.IGNORECASE), "status": ""}))
                    print(results)
                else:
                    results1=self.toList(field.find({k: re.compile(regex, re.IGNORECASE),  "status": ""}))
                    results=[d for d in results if d in results1]
            else:
                for vv in v:
                    regex='^'+ vv
                    if (not results) and (list_keys.index(k) == 0):
                        results=self.toList(field.find({k: re.compile(regex, re.IGNORECASE),  "status": ""}))
                    else:
                        results1=self.toList(field.find({k: re.compile(regex, re.IGNORECASE),  "status": ""}))
                        results=[d for d in results if d in results1]
        new_results = []
        for i in range(len(results)): 
            if results[i] not in results[i + 1:]: 
                new_results.append(results[i]) 
        return new_results


    def addLogs(self, user, message):
        day = date.today().strftime("%d/%m/%Y")
        time = datetime.now().strftime("%H:%M:%S")
        msg ={"user": user, "day":day, "time":time,"message":message }
        self.history.insert_one(msg)

    def getDates(self):
        print(self.logindates.find_one())
        return self.logindates.find_one()


    #statistics
    def updateImageBytes1(self):
        patientData =pd.DataFrame.from_dict(self.fetchPatients())
        genderData= patientData
        genderData['count'] = 1
        fig = px.pie(genderData, values='count', names='gender', title='Gender Distribution', color_discrete_sequence=px.colors.sequential.RdBu)
        fig_json = fig.to_json()
        png = plotly.io.to_image(fig)
        png_base64 = base64.b64encode(png).decode('ascii')
        print("HELLOOOO", png_base64)
        self.statistics.update_one({"number":1},{ "$set": { "image_bytes": png_base64 }})

    
    def updateImageBytes2(self):
        patientData =pd.DataFrame.from_dict(self.fetchPatients())
        patientData['count']=1
        print(patientData)
        lst_col='diseases'
        diseasePatient= pd.DataFrame({col:np.repeat(patientData[col].values, patientData[lst_col].str.len()) for col in patientData.columns.drop(lst_col)}).assign(**{lst_col:np.concatenate(patientData[lst_col].values)})[patientData.columns]        
        fig = px.pie(diseasePatient, values='count', names='diseases', title='Risk Factor Distribution', color_discrete_sequence=px.colors.sequential.RdBu)
        png = plotly.io.to_image(fig)
        png_base64 = base64.b64encode(png).decode('ascii')
        self.statistics.update_one({"number":2},{ "$set": { "image_bytes": png_base64 }})

    def updateImageBytes3(self):
        ecgData =pd.DataFrame.from_dict(self.fetchECGPatientRecords())
        ecgData=ecgData[['dateOfAcquisition','timeOfAcquisition','numberOfRRIntervals','bpm','patientId','user','machineDiagnosisKey', 'machineDiagnosisBoolean' ]]
        lst_col1='machineDiagnosisKey'
        lst_col2='machineDiagnosisBoolean'
        diagnosisData1= pd.DataFrame({col1:np.repeat(ecgData[col1].values, ecgData[lst_col1].str.len()) for col1 in ecgData.columns.drop(lst_col1)}).assign(**{lst_col1:np.concatenate(ecgData[lst_col1].values)})[ecgData.columns]
        diagnosisData2= pd.DataFrame({col2:np.repeat(ecgData[col2].values, ecgData[lst_col2].str.len()) for col2 in ecgData.columns.drop(lst_col2)}).assign(**{lst_col2:np.concatenate(ecgData[lst_col2].values)})[ecgData.columns]
        diagnosisData= diagnosisData1.assign(machineDiagnosisBoolean=diagnosisData2['machineDiagnosisBoolean'])
        diagnosisData['count']= 1
        abnormalities= diagnosisData[["machineDiagnosisKey", "count"]]
        fig = px.pie(abnormalities, values='count', names='machineDiagnosisKey', title='ECG Abnormality Distribution', color_discrete_sequence=px.colors.sequential.RdBu)
        png = plotly.io.to_image(fig)
        png_base64 = base64.b64encode(png).decode('ascii')
        self.statistics.update_one({"number":3},{ "$set": { "image_bytes": png_base64 }})

    def updateImageBytes4(self):
        ecgData =pd.DataFrame.from_dict(self.fetchECGPatientRecords())
        ecgData=ecgData[['dateOfAcquisition','timeOfAcquisition','numberOfRRIntervals','bpm','patientId','user','machineDiagnosisKey', 'machineDiagnosisBoolean',  'humanDiagnosis' ]]
        lst_col1='machineDiagnosisKey'
        lst_col2='machineDiagnosisBoolean'
        diagnosisData1= pd.DataFrame({col1:np.repeat(ecgData[col1].values, ecgData[lst_col1].str.len()) for col1 in ecgData.columns.drop(lst_col1)}).assign(**{lst_col1:np.concatenate(ecgData[lst_col1].values)})[ecgData.columns]
        diagnosisData2= pd.DataFrame({col2:np.repeat(ecgData[col2].values, ecgData[lst_col2].str.len()) for col2 in ecgData.columns.drop(lst_col2)}).assign(**{lst_col2:np.concatenate(ecgData[lst_col2].values)})[ecgData.columns]
        diagnosisData= diagnosisData1.assign(machineDiagnosisBoolean=diagnosisData2['machineDiagnosisBoolean'])
        diagnosisData['count']= 1 
        corrData= diagnosisData[['machineDiagnosisKey', 'machineDiagnosisBoolean']]
        def f1(row):
            if row["machineDiagnosisBoolean"] == "True":
                return 1
            else:
                return 0
        def f2(row):
            if row["machineDiagnosisBoolean"] == "True":
                return 0
            else:
                return 1
        corrData["hit"] = corrData.apply(f1, axis=1)
        corrData["missed"] = corrData.apply(f2, axis=1)

        corrData= corrData[['machineDiagnosisKey', 'hit', 'missed']]
        corrData=corrData.groupby('machineDiagnosisKey', as_index=False).mean().transpose()

        l_labels=list(corrData.loc["machineDiagnosisKey",: ])
        l_hit=list(corrData.loc["hit",: ])
        l_miss= list(corrData.loc["missed",: ])
        print(l_labels)
        print(l_hit)
        print(l_miss)

        data = [go.Bar(x=l_labels, y=l_hit, name="Hits", marker_color= '#f4a484'),
            go.Bar(x=l_labels, y=l_miss, name="Misses", marker_color= '#b41c2c')]
        layout = go.Layout(barmode='stack', title = 'Machine VS Human Diagnosis')
        fig = go.Figure(data=data, layout=layout)
        png = plotly.io.to_image(fig)
        png_base64 = base64.b64encode(png).decode('ascii')
        self.statistics.update_one({"number":4},{ "$set": { "image_bytes": png_base64 }})

    def updateImageBytes5(self):
        userData =pd.DataFrame.from_dict(self.fetchAccounts())
        roleData=userData[["role"]]
        roleData['count']=1
        roleData["role"]=list(roleData["role"].str.lower())
        fig = px.pie(roleData, values='count', names='role', title='Users Role Distribution', color_discrete_sequence=px.colors.sequential.RdBu)
        png = plotly.io.to_image(fig)
        png_base64 = base64.b64encode(png).decode('ascii')
        self.statistics.update_one({"number":5},{ "$set": { "image_bytes": png_base64 }})


    def updateImageBytes6(self):
        logins_json=self.getDates()
        logins_json["dates"].sort(key = lambda date: datetime.strptime(date, '%d/%m/%Y'))
        loginsData =pd.DataFrame(logins_json["dates"], columns=['dates'])
        loginsData['count']=1
        loginsData['dates'] = pd.to_datetime(loginsData['dates'], dayfirst=True)
        loginsPerDay = loginsData['dates'].dt.date.value_counts().sort_index().reset_index()
        loginsPerDay.columns = ['dates','count']
        layout = go.Layout( title = 'User Activity')
        fig = go.Figure(data=[{'x': loginsPerDay["dates"], 'y': list(loginsPerDay["count"]), 'marker_color':'#f4a484'}], layout=layout)
        png = plotly.io.to_image(fig)
        png_base64 = base64.b64encode(png).decode('ascii')
        self.statistics.update_one({"number":6},{ "$set": { "image_bytes": png_base64 }})


    def getStatistic(self, number):
        if(number ==1):
            self.updateImageBytes1()
            return self.statistics.find_one({"number": 1})
        elif(number==2):
            self.updateImageBytes2()
            return self.statistics.find_one({"number": 2})
        elif(number ==3):
            self.updateImageBytes3()
            return self.statistics.find_one({"number": 3})
        elif (number ==4):
            self.updateImageBytes4()
            return self.statistics.find_one({"number": 4})
        elif(number==5):
            self.updateImageBytes5()
            return self.statistics.find_one({"number": 5})
        elif(number ==6):
            self.updateImageBytes6()
            return self.statistics.find_one({"number": 6})
        else:
            return ""