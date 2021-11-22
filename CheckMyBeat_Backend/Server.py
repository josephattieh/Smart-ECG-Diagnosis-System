from flask import Flask
from flask import json , request, jsonify
from Database import *
from Diagnosis import *
from bson.json_util import dumps
from flask import send_file

app = Flask(__name__)
app.config["DEBUG"] = True
db = Database(Parameters.port, Parameters.databaseName)
diagnosis = Diagnosis("../QT train/qt-database-1.0.0/")
db.empty()
db.initialize()
db.populatePatientLessEcgs();

@app.route('/empty', methods=['GET'])
def empty():
    db.empty()
    return dumps({"status":"success"})

@app.route('/users/username', methods=['POST'])
def checkUsernameAvailability():
    req_data = request.json
    return dumps(db.checkUsernameAvailability(req_data["username"]))

@app.route('/users/email', methods=['POST'])
def checkEmailAvailability():
    req_data = request.json
    return dumps(db.checkEmailAvailability(req_data["email"]))

@app.route('/users/authenticate', methods=['POST'])
def authenticate():
    req_data = request.json
    username = str(req_data["username"])
    password = str(req_data["password"])
    authenticate.username = username
    return dumps(db.authenticate(username,password))

@app.route('/users/request/add', methods=['POST'])
def requestAccount():
    req_data = request.json
    username = req_data["username"]
    password=req_data["password"]
    email=req_data["email"]
    name=req_data["name"]
    gender=req_data["gender"]
    dateOfBirth=req_data["dateOfBirth"]
    role=req_data["role"]
    roleDescription=req_data["roleDescription"]
    picture=req_data["picture"]
    return dumps(db.requestAccount(username, password, email, name, gender, dateOfBirth, role, roleDescription, picture))


@app.route('/users/request/list', methods=['POST'])
def getRequests():
   return dumps(db.fetchAccountCreationRequests())  


@app.route('/users/request/approve', methods=['POST'])
def approveRequest():
    #do we add a condition that the person calling this must be an admin?
    req_data = request.json
    username = req_data["username"]
    try:
        myUsername = req_data["myUsername"]
    except:
        myUsername = authenticate.username
    db.addLogs(myUsername, "Approved "+username+" account request.");
    return dumps(db.approveRequest(username))

@app.route('/users/request/reject', methods=['POST'])
def rejectRequest():
    #do we add a condition that the person calling this must be an admin?
    req_data = request.json
    username = req_data["username"]
    reasonRejected = req_data["reasonRejected"]
    try:
        myUsername = req_data["myUsername"]
    except:
        myUsername = authenticate.username
    db.addLogs(myUsername, "Rejected "+username+" account request.");
    return dumps(db.rejectRequest(username, reasonRejected))


@app.route('/users/account/list', methods=['POST'])
def getUsers():
    return dumps(db.fetchAccounts())  

@app.route('/users/account/get', methods=['POST'])
def getUserAccount():
    username = request.json["username"]
    return dumps(db.getUserAccount(username))  

@app.route('/users/account/delete', methods=['POST'])
def deleteUserAccount():
    username = request.json["username"]
    return dumps(db.deleteUserAccount(username))  
    
@app.route('/users/account/update', methods=['POST'])
def updateUserAccount():
    req_data = request.json
    username = req_data["username"]
    password=req_data["password"]
    email=req_data["email"]
    name=req_data["name"]
    gender=req_data["gender"]
    dateOfBirth=req_data["dateOfBirth"]
    roleDescription=req_data["roleDescription"]
    picture=req_data["picture"]

    
    db.addLogs(username, "Updated Profile.");
    return dumps(db.updateUserAccount(username, password, email, name, gender, dateOfBirth, roleDescription, picture))

@app.route('/users/rolechange/request', methods=['POST'])
def requestRoleChange():
    req_data = request.json
    username = req_data["username"]
    newRole = req_data["newRole"]
    reason = req_data["reason"]
    db.addLogs(username, "Role change requested.");

    return dumps(db.requestRoleChange(username, newRole, reason))

@app.route('/users/rolechange/list', methods=['POST'])
def fetchRoleChangeRequests():
    return dumps(db.fetchRoleChangeRequests())

@app.route('/users/rolechange/approve', methods=['POST'])
def approveRoleChangeRequest():
    req_data = request.json
    username = req_data["username"]
    myUsername = req_data["myUsername"]
    
    db.addLogs(myUsername, "Approved "+username+" role change request.");
    return dumps(db.approveRoleChangeRequest(username, myUsername))

@app.route('/users/rolechange/reject', methods=['POST'])
def rejectRoleChangeRequest():
    req_data = request.json
    username = req_data["username"]
    reason = req_data["reason"]
    myUsername = req_data["myUsername"]
  
    db.addLogs(myUsername, "Reject "+username+" role change request.");
    return dumps(db.rejectRoleChangeRequest(username, reason, myUsername))

@app.route('/patients/list', methods=['POST'])
def fetchPatients():
    return dumps(db.fetchPatients())

@app.route('/patients/add', methods=['POST'])
def addPatient():
    req_data = request.json
    patientId = req_data["patientId"]
    name = req_data["name"]
    gender = req_data["gender"]
    picture = req_data["picture"]
    diseases = req_data["diseases"]
    bloodType = req_data["bloodType"]

    try:
        myUsername = req_data["myUsername"]
    except:
        myUsername = authenticate.username
    db.addLogs(myUsername, "Added patient "+patientId);
    return dumps(db.addPatient(patientId,name,gender,picture,bloodType,diseases))

@app.route('/patients/get', methods=['POST'])
def getPatient():
    req_data = request.json
    patientId = req_data["patientId"]
    return dumps(db.getPatient(patientId))

@app.route('/patients/delete', methods=['POST'])
def deletePatient():
    req_data = request.json
    patientId = req_data["patientId"]
    try:
        myUsername = req_data["myUsername"]
    except:
        myUsername = authenticate.username
    db.addLogs(myUsername, "Deleted patient "+patientId);
    return dumps(db.deletePatient(patientId))

@app.route('/patients/update', methods=['POST'])
def updatePatient():
    req_data = request.json
    patientId = req_data["patientId"]
    name = req_data["name"]
    gender = req_data["gender"]
    picture = req_data["picture"]
    diseases = req_data["diseases"]
    bloodType = req_data["bloodType"]
    try:
        myUsername = req_data["myUsername"]
    except:
        myUsername = authenticate.username
    db.addLogs(myUsername, "Updated patient "+patientId);
    return dumps(db.updatePatient(patientId,name,gender,picture,bloodType,diseases))

@app.route('/ecgs/patientless/list', methods=['POST'])
def fetchPatientlessECGs():
    return dumps(db.fetchPatientlessECGs())

@app.route('/ecgs/patientless/add', methods=['POST'])
def addPatientlessECG():
    req_data = request.json
    ecgId = req_data["ecgId"]
    data = req_data["data"]
    fs = req_data["fs"]
    picture = req_data["picture"]
    return dumps(db.addPatientlessECG( ecgId, data,fs, picture))

@app.route('/ecgs/patientless/get', methods=['POST'])
def getPatientlessECG( ):
    req_data = request.json
    ecgId = req_data["ecgId"]
    return dumps(db.getPatientlessECG(ecgId))

@app.route('/ecgs/patientless/delete', methods=['POST'])
def removePatientlessECG( ):
    req_data = request.json
    ecgId = req_data["ecgId"]
    return dumps(db.removePatientlessECG(ecgId))

@app.route('/ecgs/patientrecords/list', methods=['POST'])
def fetchECGPatientRecords():
    return dumps(db.fetchECGPatientRecords())
    
@app.route('/ecgs/patientrecords/get/patient', methods=['POST'])
def getECGsByPatientId( ):
    req_data = request.json
    patientId = req_data["patientId"]
    return dumps(db.getECGsByPatientId(patientId))

@app.route('/ecgs/patientrecords/get/ecg', methods=['POST'])
def getECGRecordByECGId( ):
    req_data = request.json
    ecgId = req_data["ecgId"]
    return dumps(db.getECGRecordByECGId( ecgId))

@app.route('/ecgs/patientrecords/delete', methods=['POST'])
def deleteECGfromPatientRecord():
    req_data = request.json
    ecgId = req_data["ecgId"]
    return dumps(db.deleteECGfromPatientRecord( ecgId))

@app.route('/ecgs/patientrecords/update', methods=['POST'])
def updateECGData():
    req_data = request.json
    ecgId = req_data["ecgId"]
    bpm = req_data["bpm"]
    pExtracted = req_data["pExtracted"]
    qExtracted = req_data["qExtracted"]
    rExtracted = req_data["rExtracted"]
    sExtracted = req_data["sExtracted"]
    tExtracted = req_data["tExtracted"]
    machineDiagnosisKey = req_data["machineDiagnosisKey"]
    machineDiagnosisProba = req_data["machineDiagnosisProba"]
    machineDiagnosisBoolean = req_data["machineDiagnosisBoolean"]
    humanDiagnosis = req_data["humanDiagnosis"]
    
    try:
        myUsername = req_data["myUsername"]
    except:
        myUsername = authenticate.username
    db.addLogs(myUsername, "Updated Diagnosis "+ecgId);
    return dumps(db.updateECGData(ecgId, bpm,pExtracted,qExtracted,rExtracted,sExtracted,tExtracted, machineDiagnosisKey,machineDiagnosisProba,machineDiagnosisBoolean, humanDiagnosis ))

@app.route('/ecgs/patientrecords/setpatient', methods=['POST'])
def associateECGtoPatient():
    req_data = request.json
    ecgId = req_data["ecgId"]
    patientId = req_data["patientId"]
    myUsername = req_data["myUsername"]
    try:
        myUsername = req_data["myUsername"]
    except:
        myUsername = authenticate.username
    db.addLogs(myUsername, "Associated ECG to patient "+ecgId+" "+patientId);
    return dumps(db.associateECGtoPatient( ecgId, patientId, myUsername))

@app.route("/ecg/patientless/random", methods=["POST"])
def retrieveRandom():
    req_data = request.json
    numberOfRRIntervals= req_data["numberOfRRIntervals"]
    numberOfSamples= req_data["numberOfSamples"]
    return dumps(diagnosis.retrieveRandom(numberOfRRIntervals, numberOfSamples))

@app.route("/ecg/patientrecords/getfeatures", methods=["POST"])
def annotate():
    req_data = request.json
    values = req_data["data"]
    numberOfRRIntervals = req_data["numberOfRRIntervals"]
    return dumps(diagnosis.annotate(values, numberOfRRIntervals))

@app.route("/ecg/patientrecords/getdiagnosis/features", methods=["POST"])
def getDiagnosisV1():
    print("Called")
    req_data = request.json
    pExtracted = req_data["pExtracted"]
    qExtracted = req_data["qExtracted"]
    rExtracted = req_data["rExtracted"]
    sExtracted = req_data["sExtracted"]
    tExtracted = req_data["tExtracted"]
    ecgId = req_data["ecgId"]
    record = db.getECGRecordByECGId(ecgId)[0]
    values = record["data"]
    numberOfRRIntervals = record["numberOfRRIntervals"]
    fs = record["fs"]
    record = diagnosis.annotate(values, numberOfRRIntervals)
    baselines = record["baselines"]
    peaks=record["peaks"]
    return dumps(diagnosis.getDiagnosisV2( values,fs,numberOfRRIntervals,pExtracted, qExtracted,rExtracted,sExtracted,tExtracted,baselines, peaks))

@app.route("/ecg/patientrecords/getdiagnosis/nofeatures", methods=["POST"])
def getDiagnosisV2():
    req_data = request.json
    values = req_data["data"]
    numberOfRRIntervals = req_data["numberOfRRIntervals"]
    fs = req_data["fs"]

    results = diagnosis.annotate(values, numberOfRRIntervals)
    pExtracted = results["pExtracted"]
    qExtracted = results["qExtracted"]
    rExtracted = results["rExtracted"]
    sExtracted = results["sExtracted"]
    tExtracted = results["tExtracted"]
    baselines = results["baselines"]
    peaks= results["peaks"]

    return dumps(diagnosis.getDiagnosisV2( values,fs,numberOfRRIntervals,pExtracted, qExtracted,rExtracted,sExtracted,tExtracted,baselines, peaks))



@app.route('/patients/search', methods=['POST'])
def dynamicSearchPatient():
    query_dict={}
    if request.json is not  None:
        print(request.json.keys())
        for k in request.json.keys():
                arg= request.json[k]
                query_dict[k]=arg
    print(query_dict)
    return dumps(db.search(query_dict, db.patients))

@app.route('/patients/search/wild', methods=['POST'])
def wildSearchPatient():
    query_dict={}
    if request.json is not  None:
        print(request.json.keys())
        for k in request.json.keys():
                arg= request.json[k]
                query_dict[k]=arg
    print(query_dict)
    return dumps(db.searchWildCard(query_dict, db.patients))

@app.route('/users/account/search', methods=['POST'])
def searchUserAccount():
    query_dict={}
    if request.json is not  None:
        for k in request.json.keys():
                arg= request.json[k]
                query_dict[k]=arg
    return dumps(db.search(query_dict, db.users)) 

@app.route('/users/account/search/wild', methods=['POST'])
def wildsearchUserAccount():
    query_dict={}
    if request.json is not  None:
        for k in request.json.keys():
                arg= request.json[k]
                query_dict[k]=arg
    return dumps(db.searchWildCard(query_dict, db.users)) 

@app.route('/users/request/search', methods=['POST'])
def searchAccountRequests():
    query_dict={}
    if request.json is not  None:
        for k in request.json.keys():
                arg= request.json[k]
                query_dict[k]=arg
    return dumps(db.searchRequests(query_dict, db.accountRequests)) 

@app.route('/users/request/search/wild', methods=['POST'])
def wildSearchAccountRequests():
    query_dict={"status": ""}
    if request.json is not  None:
        for k in request.json.keys():
                arg= request.json[k]
                query_dict[k]=arg
    return dumps(db.searchWildCard(query_dict, db.accountRequests)) 

@app.route('/users/rolechange/search', methods=['POST'])
def searchRoleChangeRequests():
    query_dict={}
    if request.json is not  None:
        for k in request.json.keys():
                arg= request.json[k]
                query_dict[k]=arg
    return dumps(db.searchRequests(query_dict, db.roleChangeRequests)) 

@app.route('/users/rolechange/search/wild', methods=['POST'])
def wildSearchRoleChangeRequests():
    query_dict={"status": ""}
    if request.json is not  None:
        for k in request.json.keys():
                arg= request.json[k]
                query_dict[k]=arg
    return dumps(db.searchWildCard(query_dict, db.roleChangeRequests)) 

@app.route('/ecgs/patientless/search', methods=['POST'])
def searchPatientlessECGs():
    query_dict={}
    if request.json is not  None:
        for k in request.json.keys():
                arg= request.json[k]
                query_dict[k]=arg
    return dumps(db.search(query_dict, db.ecgFetched))

@app.route('/ecgs/patientless/search/wild', methods=['POST'])
def wildSearchPatientlessECGs():
    query_dict={}
    if request.json is not  None:
        for k in request.json.keys():
                arg= request.json[k]
                query_dict[k]=arg
    return dumps(db.searchWildCard(query_dict, db.ecgFetched))

@app.route('/ecgs/patientrecords/search', methods=['GET', 'POST'])
def searchECGPatientRecords():
    query_dict={}
    if request.json is not  None:
        for k in request.json.keys():
                arg= request.json[k]
                query_dict[k]=arg
    return dumps(db.search(query_dict, db.ecgRecords))

@app.route('/ecgs/patientrecords/search/wild', methods=['GET', 'POST'])
def wildSearchECGPatientRecords():
    query_dict={}
    if request.json is not  None:
        for k in request.json.keys():
                arg= request.json[k]
                query_dict[k]=arg
    return dumps(db.searchWildCard(query_dict, db.ecgRecords))

@app.route('/history', methods=[ 'POST'])
def getHistory():
    req_data = request.json
    try:
        myUsername = req_data["myUsername"]
    except:
        myUsername = authenticate.username
    return dumps(db.toList(db.history.find({"user":myUsername})))

@app.route('/ecgs/patientrecords/update/patient', methods=['POST'])
def updateECGpatientOk():
    req_data = request.json
    ecgId = req_data["ecgId"]
    patientId = req_data["patientId"]
    db.ecgRecords.update_many({"ecgId": ecgId},{ "$set": { "patientId": patientId }})
    return dumps({"status": "success"})

@app.route('/users/logindates/get', methods=['GET', 'POST'])
def getUserCounter():
    return dumps(db.getDates())



@app.route('/statistics/figure/get', methods=['GET', 'POST'])
def getStatistics():
    req_data = request.json
    number = req_data["number"]
    return dumps(db.getStatistic(number))


app.run()
