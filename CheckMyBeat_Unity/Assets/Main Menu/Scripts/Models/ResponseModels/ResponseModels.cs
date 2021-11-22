public class ResponseModels { }
// @app.route('/users/rolechange/list', methods=['GET'])
// def fetchRoleChangeRequests():
public class RoleChangeRequest
{
    public string username;
    public string newRole;
    public string dateOfRequest;
    public string reasonRejected;
    public string timeOfRequest;
    public string reason;
    public string by;
}



/*return response of:
    @app.route('/empty', methods=['GET'])
    def empty():

    @app.route('/users/request/add', methods=['POST'])
    def requestAccount():

    @app.route('/users/request/approve', methods=['POST'])
    def approveRequest():

    @app.route('/users/request/reject', methods=['POST'])
    def rejectRequest():


    @app.route('/users/account/delete', methods=['POST'])
    def deleteUserAccount():
        
    @app.route('/users/account/update', methods=['POST'])
    def updateUserAccount():


    @app.route('/users/rolechange/request', methods=['POST'])
    def requestRoleChange():

    @app.route('/users/rolechange/approve', methods=['POST'])
    def approveRoleChangeRequest():

    @app.route('/users/rolechange/reject', methods=['POST'])
    def rejectRoleChangeRequest():

    @app.route('/patients/add', methods=['POST'])
    def addPatient():

    @app.route('/patients/delete', methods=['POST'])
    def deletePatient():

    @app.route('/patients/update', methods=['POST'])
    def updatePatient():

    @app.route('/ecgs/patientless/add', methods=['POST'])
    def addPatientlessECG():

    @app.route('/ecgs/patientless/delete', methods=['POST'])
    def removePatientlessECG( ):

    
    @app.route('/ecgs/patientrecords/delete', methods=['POST'])
    def deleteECGfromPatientRecord():

    @app.route('/ecgs/patientrecords/update', methods=['POST'])
    def updateECGData():

    @app.route('/ecgs/patientrecords/setpatient', methods=['POST'])
    def associateECGtoPatient():

*/

public class Status
{
    public string status; //success or failure or message
}
public class ImageString
{
    public string image_bytes;
}


//List of accountRequests
// @app.route('/users/request/list', methods=['GET'])
// def getRequests():
//Returns List of users
// @app.route('/users/account/list', methods=['GET'])
// def getUsers():
// @app.route('/users/account/get', methods=['POST'])
// def getUserAccount():

// @app.route('/users/authenticate', methods=['POST'])
// def authenticate():
public class AccountRequests
{
    public string reason;
    public string status;
    public string username;
    public string password;
    public string email;
    public string name;
    public string dateOfBirth;
    public string gender;
    public string picture;
    public string role;
    public string roleDescription;
}



//List of patients
// @app.route('/patients/list', methods=['GET'])
// def fetchPatients():
// @app.route('/patients/get', methods=['POST'])
// def getPatient():
public class Patient
{
    public string patientId;
    public string name;
    public string gender;
    public string picture;
    public string bloodType;
    public string[] diseases;
}

//ecgFetched
// @app.route('/ecgs/patientless/list', methods=['GET'])
// def fetchPatientlessECGs():
// @app.route('/ecgs/patientless/get', methods=['POST'])
// def getPatientlessECG( ):
public class EcgFetched
{
    public string ecgId;
    public float[] data;
    public string fs;
    public string numberOfRRIntervals;
    public string dateOfAcquisition;
    public string timeOfAcquisition;
    public string picture;

}

//ecgRecords
// @app.route('/ecgs/patientrecords/list', methods=['GET'])
// def fetchECGPatientRecords():
// @app.route('/ecgs/patientrecords/get/patient', methods=['POST'])
// def getECGsByPatientId( ):
// @app.route('/ecgs/patientrecords/get/ecg', methods=['POST'])
// def getECGRecordByECGId( ):
public class EcgRecords
{
    public string ecgId;
    public float[] data;
    public string fs;
    public string dateOfAcquisition;
    public string timeOfAcquisition;
    public string picture;
    public string bpm;
    public string patientId;
    public string user;
    public string numberOfRRIntervals;
    public int[] pExtracted;
    public int[] qExtracted;
    public int[] rExtracted;
    public int[] sExtracted;
    public int[] tExtracted;
    public string[] machineDiagnosisKey;
    public float[] machineDiagnosisProba;
    public bool[] machineDiagnosisBoolean;
    public string humanDiagnosis;

}

public class History
{
    public string user;
    public string day;
    public string time;
    public string message;
}

