# This script generates a calibration text file marker containing positions and angles
# which will be used by the ClientApplication
# to calibrate markers, press "c" aiming at a known point
# Every markers recognized in the picture will be calibrated according to the aimed point, you have to manually enter it position
# then repeat for each unknown marker that you want to add in the file
# when you have finish, press "s" to save the data in a text file

import numpy as np
import cv2
from cv2 import aruco
import re

print ("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #")
print ("This script generates a calibration text file marker containing positions and angles")
print ("which will be used by the ClientApplication")
print ("to calibrate markers, press \"c\" aiming at a known point")
print ("Every markers recognized in the picture will be calibrated according to the aimed point, you have to manually enter it position")
print ("then repeat for each unknown marker that you want to add in the file")
print ("when you have finish, press \"s\" to save the data in a text file")
print (" - - - - - - - - - - - - - - - - - - - - - - ")
print ("\tc - calibrate using this frame")
print ("\ts - register the file (will destroy any previouly generated file)")
print ("\tq - quit")
print ("# # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # # #")

# corrections according to the chosen axis and the camera orientation
mirrorX = -1
mirrorY = 1

# used camera identifier, usually 0 is for the PC webcam et 1 for external webcam
cameraId = 0

# calibration file name
calibrationFile = "calibration.txt"

# length in mm of the printed markers
lc = 87


def mirrorCorrection(pos):
    # applies mirror correction
    x,y = pos
    return np.array([mirrorX * x, mirrorY * y])

def drawStates(img, dict, ids, pos):
    # draws a green diamond when a marker is already registered, else a red cross on a marker corner 

    if ids is None :
        return img
    for id,p in zip(ids,pos):
        id=id[0]
        x,y = int(p[0]),int(p[1])
        if id in dict:
            # if the marker is already registered and belongs to the dictionary
            img = cv2.drawMarker(img,(x,y),color= (0,255,0),markerType=cv2.MARKER_DIAMOND,markerSize=20,thickness=3)
        else:
            img = cv2.drawMarker(img,(x,y),color= (0,0,255),markerType=cv2.MARKER_TILTED_CROSS,markerSize=20,thickness=3)
    return img

def calibrate(dict, ids, markersPxPos, cameraMmPos, centrePxPos, mmPerPx, anglesMarkers):
    # computes position and angle for all the markers in the image, then returns a dictionary of them
    #### VERY IMPORTANT ####
    # to succeed the calibration, the camera angle needs to be 0°
    for id,posPx,angle in zip(ids, markersPxPos, anglesMarkers):
        id=id[0]
        pos =np.array(cameraMmPos + mirrorCorrection(posPx - centrePxPos) * mmPerPx, dtype=np.int16)
        dict[id] = (pos,angle)
    return dict

def askCameraPosition(msg="Enter camera position in mm as 'x,y' : "):
    pos=input(msg)
    datas = re.match(r"-?[0-9]+,-?[0-9]+",pos)
    if datas:
        text = datas.group(0)
        x, y = text.split(",")
        x, y = int(x), int(y)
        return(x,y)
    else:
        return askCameraPosition("Bad format. You have to enter 'x,y'\n")

def saveCalibration(dictionary, path):
    # writes on a text file the datas of the dictionary
    f = open(path, "w+")
    f.write("id x y theta\n\n")
    for key in dictionary.keys():
        x = str((dictionary[key])[0][0])
        y = str((dictionary[key])[0][1])
        a = str(int((dictionary[key])[1]))
        f.write(str(key)+" "+x+" "+y+" "+a+"\n")
    f.close()
    print("File saved")



cap = cv2.VideoCapture(cameraId)
w = cap.get(3)
h = cap.get(4)
centre = np.array([w // 2, h // 2]) # centre de l'image

# property dictionary, each marker position/angle
markersPositions = {}
# marker id (int) : (position ([int,int]), angle (int))

# chosen dictionary of ARuco markers
arucoDict = aruco.Dictionary_get(aruco.DICT_4X4_50)
parameters = aruco.DetectorParameters_create()

# initialisation
n = arucoDict.bytesList.size
anglesMarkers = np.zeros(n)

while(True):
    # Each loop is a frame
    ret, frame = cap.read()

    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    # markers detection
    corners, ids, rejectedImgPoints = aruco.detectMarkers(gray, arucoDict, parameters=parameters)

    c = 0 # c longest length in pixels of the seen markers
    nb = len(corners) # number of detected markers
    try:
        angles = np.zeros(nb)
        pos = np.zeros((nb,2))

        for i,corner in enumerate(corners):
            vector = corner[0][0] - corner[0][3] + corner[0][1] - corner[0][2]
            angles[i] = (np.arctan2(-vector[1], vector[0]) * 180 / np.pi - anglesMarkers[ids[i]]) % 360
            pos[i]=corner[0][0]

            cTmp = int(np.linalg.norm(vector) / 2)
            if c < cTmp:
                c = cTmp

        ratioMmPerPx = lc / c # mm / pixel


    except:
        pass

    # draws detected Aruco markers
    frameDisplayed = aruco.drawDetectedMarkers(frame.copy(), corners, ids)

    # draws a circle in the middle on the image, it is better to target a precise point with the camera
    frameDisplayed=cv2.circle(frameDisplayed, (int(w // 2), int(h // 2)), 10, (255, 0, 0))

    try:
        frameDisplayed = drawStates(frameDisplayed, markersPositions, ids, pos)
        cv2.imshow('frame', frameDisplayed)
    except:
        pass

    if cv2.waitKey(1) & 0xFF == ord('c'):
        position_caméra_donnée = askCameraPosition()
        markersPositions = calibrate(markersPositions, ids, pos, position_caméra_donnée, centre, ratioMmPerPx, angles)
    elif cv2.waitKey(1) & 0xFF == ord('s'):
        saveCalibration(markersPositions, calibrationFile)
    elif cv2.waitKey(1) & 0xFF == ord('q'):
        break

# When everything done, release the capture
cap.release()
cv2.destroyAllWindows()

