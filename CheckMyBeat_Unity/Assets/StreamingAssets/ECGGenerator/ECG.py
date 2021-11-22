#imports 
from math import sin, cos, pi
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd
import sys
import pathlib
import os

#Argument 1 defines if default values are shown or not (if 1, then use default values, else use parameters)


def p_wav(x,a,d_pwav,t_pwav,li):
    x=x+t_pwav;
    b=(2*li)/d_pwav;
    n=100;
    p2=0;
    for i in range(1,n+1):
        p2+=np.array([(((sin((pi/(2*b))*(b-(2*i))))/(b-(2*i))+(sin((pi/(2*b))*(b+(2*i))))/(b+(2*i)))*(2/pi))*cos((i*pi*xx)/li) for xx in x])
    return a*(1/li+p2)

def q_wav(x,a,d_qwav,t_qwav,li):
    x=x+t_qwav;
    b=(2*li)/d_qwav;
    n=100;
    q1=(a/(2*b))*(2-b);
    q2=0;
    for i in range(1,n+1):
        q2+=np.array([((2*b*a)/(i*i*pi*pi))*(1-cos((i*pi)/b))*cos((i*pi*xx)/li) for xx in  x]);
    return -1*(q1+q2);

def qrs_wav(x,a,d_qrswav,li):
    b=(2*li)/d_qrswav;
    n=100;
    qrs1=(a/(2*b))*(2-b);
    qrs2=0;
    for i in range(1,n+1):
        qrs2+=np.array([(((2*b*a)/(i*i*pi*pi))*(1-cos((i*pi)/b)))*cos((i*pi*xx)/li) for xx in x]);
    return qrs1+qrs2

def s_wav(x,a,d_swav,t_swav,li):
    x=x-t_swav;
    a=a;
    b=(2*li)/d_swav;
    n=100;
    s1=(a/(2*b))*(2-b);
    s2=0;
    for i in range(1,n+1):
        s2+=np.array([(((2*b*a)/(i*i*pi*pi))*(1-cos((i*pi)/b)))*cos((i*pi*xx)/li) for xx in x]);
    return -1*(s1+s2);


def t_wav(x,a,d_twav,t_twav,li):
    x=x-t_twav-0.045;
    b=(2*li)/d_twav;
    n=100;
    t2=0;
    for i in range(1,n+1):
        t2+=np.array([(((sin((pi/(2*b))*(b-(2*i))))/(b-(2*i))+(sin((pi/(2*b))*(b+(2*i))))/(b+(2*i)))*(2/pi))*cos((i*pi*xx)/li) for xx in x]);             
    return a*(1/li+t2)


def u_wav(x,a,d_uwav,t_uwav,li):
    x=x-t_uwav;
    b=(2*li)/d_uwav;
    n=100;
    u2=0
    for i in range(1,n+1):
        u2+=np.array([(((sin((pi/(2*b))*(b-(2*i))))/(b-(2*i))+(sin((pi/(2*b))*(b+(2*i))))/(b+(2*i)))*(2/pi))*cos((i*pi*xx)/li) for xx in x]);             
    return a*(1/li+u2)




x=np.array([a for a in list(range(1, 200))] )/100 #0.01 to 2 with 0.01 = 1 to 200 with 1
default= float(sys.argv[1])
if(default==1):
      li=30/72;  
      a_pwav=0.25;
      d_pwav=0.09;
      t_pwav=0.16;  
      a_qwav=0.025;
      d_qwav=0.066;
      t_qwav=0.166;      
      a_qrswav=1.6;
      d_qrswav=0.11;
      a_swav=0.25;
      d_swav=0.066;
      t_swav=0.09;
      a_twav=0.35;
      d_twav=0.142;
      t_twav=0.2;
      a_uwav=0.035;
      d_uwav=0.0476;
      t_uwav=0.433;
else:
    # Argv starts at 1 not at 0 !!
    rate= float(sys.argv[2])
    li=30/rate;
    a_pwav=float(sys.argv[3])
    d_pwav=float(sys.argv[4])
    t_pwav=float(sys.argv[5])

    a_qwav=float(sys.argv[6])
    d_qwav=float(sys.argv[7])
    t_qwav=0.166;

    a_qrswav=float(sys.argv[8])
    d_qrswav=float(sys.argv[9])

    a_swav=float(sys.argv[10])
    d_swav=float(sys.argv[11])
    t_swav=0.09;

    a_twav=float(sys.argv[12]);
    d_twav=float(sys.argv[13]);
    t_twav=float(sys.argv[14])

    a_uwav=float(sys.argv[15])
    d_uwav=float(sys.argv[16])
    t_uwav=0.433;
    
        
pwav=p_wav(x,a_pwav,d_pwav,t_pwav,li);
qwav=q_wav(x,a_qwav,d_qwav,t_qwav,li);
qrswav=qrs_wav(x,a_qrswav,d_qrswav,li);
swav=s_wav(x,a_swav,d_swav,t_swav,li);
twav=t_wav(x,a_twav,d_twav,t_twav,li);
uwav=u_wav(x,a_uwav,d_uwav,t_uwav,li);
ecg=pwav+qrswav+twav+swav+qwav+uwav;
df =pd.DataFrame()
df["time"] =x
df["ECG"] = ecg
plt.plot(x, ecg, color ='black')
ax = plt.gca()
ax.set_facecolor('#f7cac9')
plt.grid(color = '#f26052', linestyle='-.', linewidth=0.4)
#For debug use this
cwd = os.path.abspath(os.getcwd())
df.T.to_csv( cwd + "\Assets\StreamingAssets\ECGOutput\ECG.csv")

#For publish use this
#df.T.to_csv( cwd + "\StreamingAssets\ECGOutput\ECG.csv")

#For debug use this
plt.savefig(cwd + "\Assets\StreamingAssets\ECGOutput\ECG.png")

#For publish use this
#plt.savefig(cwd + "\StreamingAssets\ECGOutput\ECG.png")


#plt.show(block=True)

#DONT DELETE THIS!!
print("FinishedProcess");


