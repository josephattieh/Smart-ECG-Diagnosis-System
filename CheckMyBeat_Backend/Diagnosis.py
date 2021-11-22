import wfdb, random,re,os
from random import  randint
from scipy.signal import gaussian, decimate,find_peaks
import numpy as np
from ecgdetectors import Detectors
import time
from sklearn.cluster import KMeans
import peakutils
from  scipy import interpolate


class Diagnosis():

    def __init__(self, qtLocation):
        #self.qtLocation = qtLocation
        #self.qtData = self.getQtRecords()
        np.random.seed(42)
        random.seed()

    def getQtRecords(self):
        return sorted(list(set([a[:a.index(".")] for a in os.listdir(self.qtLocation) if re.match("[a-z0-9]+\.[a-z]+",a)])))
        
    def getRecordData(self, dataNbr):  
        #dataNbr is a value from the qtRecords!!!!!!!!!! Okay guy??
        print(self.qtLocation+str(dataNbr))
        record = wfdb.rdsamp(self.qtLocation+str(dataNbr))
        annotation = wfdb.rdann(self.qtLocation+str(dataNbr),"pu" )
        toReturn=wfdb.rdheader(self.qtLocation+str(dataNbr)).__dict__
        toReturn["dataNbr"] =str(dataNbr)
        toReturn["annotation"] = {annotation.__dict__["sample"][i]: annotation.__dict__["symbol"][i] for i in range(len(annotation.__dict__["sample"]))}
        toReturn["sig0"] = record[0][:, 0]
        toReturn["sig1"] = record[0][:, 1]
        return toReturn
    
 
    def retrieveRandom(self, numberOfRRIntervals, numberOfSamples):

        numberOfRRIntervals = int(numberOfRRIntervals)
        if(numberOfRRIntervals<=0):
            numberOfRRIntervals =1

        record = self.getRecordData(random.sample(self.qtData, 1)[0])
        signal = record["sig0"] if randint(0,1) ==0 else record["sig1"]
        print(len(signal))
        possibleOnes =HelperClass.getRRInterval(record["fs"], signal, numberOfRRIntervals,1.05)
        toReturn  = random.sample(possibleOnes, numberOfSamples)
#         for dataIndexes in toReturn:
#             fig = plt.figure()
#             plt.plot(np.linspace(dataIndexes[0],dataIndexes[1], dataIndexes[1]-dataIndexes[0]),signal[dataIndexes[0]:dataIndexes[1]].tolist() )
#             for s in record["annotation"].keys():
#                 if s< dataIndexes[1] and s> dataIndexes[0]:
#                     plt.annotate(record["annotation"][s], (s,signal[s]),   textcoords="offset points",  xytext=(0,10), ha='center') 

        tR =[{"data":signal[dataIndexes[0]:dataIndexes[1]].tolist(),"numberOfRRIntervals":numberOfRRIntervals, "fs": record["fs"], "picture":"", "ecgId":HelperClass.getHashId()} for dataIndexes in toReturn]
        return tR
    
    
    def annotate(self, values,numberOfRRIntervals):
        RRIntervals  =HelperClass.splitByRRIntervals(values,numberOfRRIntervals )
        results ={"pExtracted":[], "qExtracted":[], "rExtracted":[],"sExtracted":[], "tExtracted":[], "baselines":[], "peaks":[], "numberOfRRIntervals":numberOfRRIntervals}
        oneInterval = len(values)/numberOfRRIntervals;
        for i in range(len(RRIntervals)):
            interval = RRIntervals[i]
            [p,q,r,s,t,baseline,peaks]= HelperClass.annotateOneRR( interval)
            results["pExtracted"].append(int(p+oneInterval*i))
            results["qExtracted"].append(int(q+oneInterval*i))
            results["rExtracted"].append(int(r+oneInterval*i))
            results["sExtracted"].append(int(s+oneInterval*i))
            results["tExtracted"].append(int(t+oneInterval*i))
            results["baselines"].append(baseline)
            results["peaks"].append([int(peak+oneInterval*i) for peak in peaks])
        
       
        for k in ["pExtracted", "qExtracted","rExtracted", "sExtracted", "tExtracted"]:
            results[k] = [0 if a<0 else a for a in results[k]]
                
        return results
    
    def getDiagnosis(self, values,fs, results):
        diag =[]
        oneInterval = len(values)/results["numberOfRRIntervals"];

        for i in range(len(results["pExtracted"])):
            p= results["pExtracted"][i] 
            q= results["qExtracted"][i]
            r= results["rExtracted"][i]
            s= results["sExtracted"][i]
            t= results["tExtracted"][i]
            baseline = results["baselines"][i]
            peak = results["peaks"][i]
            res = HelperClass.generateOneResultDiagnosis(values,peak, p,q,r,s,t,fs,baseline,numberOfRRIntervals)
            diag+=(res["diagnosis"])

        diseases =list(set(diag))
        probabilities =[ (diag.count(d)*100)/len(results["pExtracted"]) for d in diseases]
        validated =[False for d in diseases]
        results["machineDiagnosisKey"] = diseases
        results["machineDiagnosisProba"] = probabilities
        results["machineDiagnosisBoolean"] = validated
        results["bpm"] = res["bpm"]
        return results

    def getDiagnosisV2(self, values,fs,numberOfRRIntervals,pExtracted, qExtracted,rExtracted,sExtracted,tExtracted,baselines, peaks):
        diag =[]
        oneInterval = len(values)/numberOfRRIntervals;

        for i in range(len(pExtracted)):
            p= pExtracted[i] 
            q= qExtracted[i]
            r= rExtracted[i]
            s= sExtracted[i]
            t= tExtracted[i]
            baseline = baselines[i]
            peak = peaks[i]
            res = HelperClass.generateOneResultDiagnosis(values,peak, p,q,r,s,t,fs,baseline,numberOfRRIntervals)
            diag+=(res["diagnosis"])

        diseases =list(set(diag))
        probabilities =[ (diag.count(d)*100)/len(pExtracted) for d in diseases]
        validated =[False for d in diseases]
        results ={"pExtracted":pExtracted, "qExtracted":qExtracted, "rExtracted":rExtracted,"sExtracted":sExtracted, "tExtracted":tExtracted, "baselines":baselines, "peaks":peaks, "numberOfRRIntervals":numberOfRRIntervals}
        results["data"] = values
        results["machineDiagnosisKey"] = diseases
        results["machineDiagnosisProba"] = probabilities
        results["machineDiagnosisBoolean"] = validated
        results["bpm"] = res["bpm"]
        return results
    
class HelperClass():
    @staticmethod
    def generateOneResultDiagnosis(values,peaks, p,q,r,s,t,fs,baseline,numberOfRRIntervals):
        diagnosis =[]
        print(len(values))
        time = np.array(range(0,len(values)))/fs
        print(max(time))
        bpm =  numberOfRRIntervals/((len(time)/(fs*60)))
        if (bpm< 60): diagnosis.append("Bradycardia")
        elif (bpm<100): diagnosis.append("Normal")
        else: diagnosis.append("Tachycardia")
        
        
        # 5mm = 0.2 sec  ==> 1mm=0.04s (1 unit is 1/fs s)
        #value => 1mm = 0.1 mV (in mV)
        #>2.5mm
        if p>0:
            if(abs(values[p])> 2.5*0.1): diagnosis.append("Right atrial hypertrophy/  enlargement")
            if(values[p]<0): diagnosis.append("Either sinoatrial block or dextrocardia")
            if(abs(abs(values[p])-baseline)<0.001):diagnosis.append("Sinoatrial block")

        #PR
        if p>0:
            valueBeforeP = [ i for i in range(0, p) if abs(abs(values[i])-abs(baseline))<0.001]
            if(len(valueBeforeP)!=0):
                valueBeforeP= valueBeforeP[-1] 
            else:
                valueBeforeP=p
        #200ms
            if q>0:
                if(len(time[valueBeforeP:q])/fs>0.2):diagnosis.append("AV block")

        #QRS interval
        if q>0 and s>0:
            if(len(time[q:s])/fs>0.120):diagnosis.append("Either VFrib, VTach or Supratach")

        #QT interval
        if q>0 and t>0: 
            if(len(time[q:t]))/(len(time))>0.440:diagnosis.append("Long QT")

        if t>0:
            if(values[t])<0:diagnosis.append("Abnormal T wave ")

        return {"bpm":bpm, "diagnosis":diagnosis}
    @staticmethod
    def detectBaseline(valuesY, precision =100):
        if np.min(valuesY)<0:
            values = valuesY - np.min(valuesY)
        else:
            values = valuesY
        dic = {(i/precision):0 for i in range(int((precision+1)*(np.max(values))))}
        for v in values:
            if int(v*precision)/precision not in dic.keys():
                dic[int(v*precision)/precision]=0
            dic[int(v*precision)/precision]= dic[int(v*precision)/precision]+1
        ok = [k for k in dic.keys() if dic[k]==max(dic.values())][0]
        if np.min(valuesY)<0:
            return ok +np.min(valuesY)
        return ok
    @staticmethod
    def gaussian_smoothing(data, window, std):
        gauss = gaussian(window ,std, sym=True)
        data = np.convolve(gauss/gauss.sum(), data, mode='same')
        return data 
    
    @staticmethod
    def gauss_wrapper(data):
        return HelperClass.gaussian_smoothing(data, 12, 7)

        
    @staticmethod
    def getHashId():
        return hex(int(time.time() + 12345))[2:]
    
    @staticmethod    
    def getRRInterval( fs, values,numberOfRRIntervals, howMuch=1.2):
        print(fs)
        #smoothen values
        values = HelperClass.gauss_wrapper(values)
        values10s = values
        #make sure the highest valye in absolute value is positive (use to detect peaks)
        minimum =np.abs(np.min(values10s))
        maximum =np.abs(np.max(values10s))
        if(minimum> maximum):
            values10s = values10s*-1
        #normalize amplitude
        values = (values10s- np.min(values10s))/(np.max(values10s)-np.min(values10s))
        m= np.max(values)
        values = values/m
        print(len(values))
        detectors = Detectors(fs)
        #detect the R peaks using the Pan Tompkin algorithm
        peaks = detectors.pan_tompkins_detector(values)
        #Find median of R-R intervals as nominal heart beat period 
        T = np.median([peaks[i+1]-peaks[i] for i in range(len(peaks)-1)])
        if(minimum> maximum):
            values = values*-1
        #For each R-peak, select a signal part with the length equal to T.
        parts = [[int (peaks[i]-howMuch*T/2), int(peaks[i+numberOfRRIntervals-1]+howMuch*T/2) ] for i in range(len(peaks)-numberOfRRIntervals)]
        #make sure the lower value and upper values are positive
        parts =[ a for a in parts if a[0]>0 and a[1]>0]
        return parts
    
    @staticmethod    
    def splitByRRIntervals(values,numberOfRRIntervals ):
        return [np.array(values)[int((i)*(len(values)-1)/numberOfRRIntervals):int((i+1)*(len(values)-1)/numberOfRRIntervals)].tolist() for i in range(numberOfRRIntervals)]
    
    @staticmethod    
    def annotateOneRRT( values):
        baseline = HelperClass.detectBaseline(values)
        values = np.array(values)
        values = values-baseline
        peaks =find_peaks(values)[0].tolist()
        peaks +=find_peaks(-1*values)[0].tolist()
        peaks = sorted(list(set(peaks)))
        r_index =np.argmax(np.abs(values[peaks]))
        r = peaks[r_index]

        if values[r]<0:
            values = values*-1

        q = peaks[r_index-1]
        s = peaks[r_index+1]

        p= peaks[np.argmax(np.abs(values[peaks[:r_index-1]]))]
        t= peaks[np.argmax(np.abs(values[peaks[r_index+2:]])) +r_index+2]
        return [p,q,r,s,t, baseline, peaks]

    @staticmethod    
    def annotateOneRR(y_input):
        #remove baseline effect
        x_input = np.array(list(range(len(y_input))))
        y_input  = np.array(y_input)
        #remove baseline effect
        x=x_input+0
        baseline = peakutils.baseline(y_input)
        baseline = peakutils.baseline(y_input-baseline)
        y = y_input-baseline
        
    
        # Interpolate the data using a cubic spline to 50 samples
        new_length = 50
        new_x = np.linspace(x.min(), x.max(), new_length) [1:-2]
        new_y = interpolate.interp1d(x, y, kind='cubic')(new_x)
        diffy = np.diff(new_y)
        #get peaks of deriverative
        peaksMax = find_peaks(diffy)[0]
        orderedPeaksMax = peaksMax[np.argsort(diffy[peaksMax])][::-1]
        peaksMin  = find_peaks(diffy*-1)[0]
        orderedPeaksMin = peaksMin[np.argsort(diffy[peaksMin])]
        
        peaks = sorted(orderedPeaksMax.tolist()[:4]+orderedPeaksMin.tolist()[:4])
        peaksT =[[p] for p in peaks]
        kmeans = KMeans(n_clusters=5)
        kmeans.fit(peaksT)
        y_kmeans = kmeans.predict(peaksT).tolist()
        maxC = y_kmeans[peaksT.index(orderedPeaksMax[0])]
        peaks = sorted([peaksT[i][0] for i in range(len(peaksT)) if y_kmeans[i]==maxC])
        
        peakS =sorted(list(set(orderedPeaksMax.tolist()[:]+orderedPeaksMin.tolist()[:])))
        
        a =peakS[peakS.index(peaks[0])-1]
        b= peakS[peakS.index(peaks[-1])+1]
        a, b = np.min([a,b]), np.max([a,b])+1
        
        y = y_input
        x = x_input
        # baseline = peakutils.baseline(new_y)
        baseline = HelperClass.detectBaseline(y)
        peaks = np.array(sorted(find_peaks(new_y[a:b])[0].tolist()+find_peaks(new_y[a:b]*-1)[0].tolist())) +a+1


        d =np.diff(new_y)[a:b]
        #Nbr of peaks to annotate later on
        NbrOfpeaks =len( np.argwhere(np.diff(np.sign(d - 0.15*np.max(d)))).flatten().tolist()+np.argwhere(np.diff(np.sign(d - 0.15*np.min(d)))).flatten().tolist())/2

        new_a = a
        new_b= b
        #a and b in the x dimensions
        a = int(a*len(x)/len(new_x))
        b = int(b*len(x)/len(new_x))
        y = y-baseline
        

        peaks = np.array(sorted(find_peaks(y[a:b])[0].tolist()+find_peaks(y[a:b]*-1)[0].tolist())) + a
        #filter by value of Y
        baseline = HelperClass.detectBaseline(y)
    
        res={}
        if(NbrOfpeaks <=2):
                #either QS or R
                mX =np.argmax(y[peaks])
                mN = np.argmin(y[peaks])
                if(np.abs(y[peaks[mX]]-baseline)< np.abs(y[peaks[mN]]-baseline)):
                    res["Q"] = peaks[mN]
                    res["S"] = peaks[mN]
                else:
                    res["R"] = peaks[mX]
        elif(NbrOfpeaks <= 3):
                #either QR or RS
                mX =np.argmax(y[peaks])
                mN = np.argmin(y[peaks])
                if(y[peaks[mX]]<baseline):
                    res["Q"] = peaks[mN]
                    res["S"] = peaks[mN]

                else:
                    res["R"] = peaks[mX]

                    if(mX> mN):
                        if y[peaks[mN]]<baseline:
                            res["Q"] = peaks[mN]
                        else:
                            res["Q"] = peaks[np.argmin(y[peaks[mN+1:]])+mN+1]
                    else:
                        if y[peaks[mN]]<baseline:
                            res["S"] = peaks[mN]
                        else:
                            if len(peaks[:mN-1])!=0:
                                res["Q"] = peaks[np.argmin(y[peaks[:mN-1]])]


        else:
                    mX =np.argmax(y[peaks])
                    mN = np.argmin(y[peaks])
                    if y[peaks[mX]] > baseline:
                        res["R"] = peaks[mX]
                    if len(peaks[:mX-1])!=0:
                        q = peaks[np.argmin(y[peaks[:mX]])]
                        if (y[q]< baseline):
                            res["Q"]=q

                    if len(peaks[mX+1:])!=0:
                        s = peaks[np.argmin(y[peaks[mX+1:]])+mX+1]
                        if (y[s]< baseline):
                            res["S"]=s
                    print(res)
        y= y+baseline
        yp= new_y-HelperClass.detectBaseline(new_y)

        #new_a and new_b should be after a change of sign  before the latest peak
        new_a = int((np.min(list(res.values()))-4)*len(yp)/len(y))
        new_b = int((np.max(list(res.values()))+4)*len(yp)/len(y))
        
        peaksP = np.array(sorted(find_peaks(yp[:new_a])[0].tolist()+find_peaks(yp[:new_a]*-1)[0].tolist())) 
        peaksT = np.array(sorted(find_peaks(yp[new_b:])[0].tolist()+find_peaks(yp[new_b:]*-1)[0].tolist())) + new_b
        
        
        changeOfP = np.argmax([abs(yp[peaksP[i]]-yp[peaksP[i-1]]) for i in range(1, len(peaksP))])
        changeOfT = np.argmax([abs(yp[peaksT[i+1]]-yp[peaksT[i]]) for i in range( len(peaksT)-1)])+1
        
        #changeP and changeT need to be adjusted
        s = changeOfP-1 if changeOfP>=1 else changeOfP
        changeOfP = np.argmax([abs(yp[a]) for a in peaksP[s: changeOfP+2]])+s

        
        s = changeOfT-1 if changeOfT>=1 else changeOfT
        changeOfT = np.argmax([abs(yp[a]) for a in peaksT[s: changeOfT+2]])+s
    
        #filter peaksP and peaksT to take into account a change of sign
        res["P"]= int(peaksP[changeOfP]*len(y)/len(yp))
        res["T"]= int(peaksT[changeOfT]*len(y)/len(yp))
        
        x= x_input
        y=y_input
        baseline = HelperClass.detectBaseline(y)

        peaks =find_peaks(y)[0].tolist()
        peaks +=find_peaks(-1*y)[0].tolist()
        peaks = sorted(list(set(peaks)))

        for k in ["P", "Q", "R", "S", "T"]:
            if k not in res.keys():
                res[k] =-10000000000000

        return [res["P"],res["Q"],res["R"],res["S"],res["T"],baseline, peaks]
