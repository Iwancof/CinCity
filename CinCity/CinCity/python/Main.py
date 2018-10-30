import cv2
import sys
import numpy as np
import socket

def Send(msg):
    client = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
    host = "127.0.0.1"
    port = 51081
    client.connect((host,port))
    msg = str(msg)
    client.send(msg.encode())

def calcMoments(corners,ids):
    moments = np.empty((len(corners),2))
    for i in range(len(corners)):
        index = int(ids[i])-1
        moments[index] = np.mean(corners[i][0],axis=0)
        return moments
#sys.argv.__add__(1280)
#sys.argv.__add__(720)
def CenterCoord(list):
    x = 0
    y = 0
    for m in list:
        x += m[0]
        y += m[1]

    return (np.int(x / 4),np.int(y / 4))

def CreateMessage(IsCenter,typestr,id,coord):
    if(IsCenter):
        return typestr + "-i{0:02d}".format(id[0]) + "c{0:03d}".format(CenterCoord(coord[0])[0]) + ",{0:03d}:".format(CenterCoord(coord[0])[1])
    else:
        return typestr + "-i{0:02d}".format(id[0]) + "c{0:03d}".format(np.int(coord[0][0][0])) + ",{0:03d}:".format(np.int(coord[0][0][1]))


cap = cv2.VideoCapture(0)

cap.set(cv2.CAP_PROP_FPS,30)
cap.set(cv2.CAP_PROP_FRAME_WIDTH,640)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT,480)

print(cap.get(cv2.CAP_PROP_FPS))
print(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
print(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))

dictionary_name = cv2.aruco.DICT_6X6_250
dictionary = cv2.aruco.getPredefinedDictionary(dictionary_name)

CriterionMarkers = [0,1,2,3]
RoadMarkers = [4,5,6,7]
BuildingMarkers = [8,9,10,11]
CleanerMarkers = [12,13,14,15]

while True:
    ret, frame = cap.read()

    #テスト用画像。場所はランダム
    #frame = cv2.imread("../bin/Debug/s6X6_250_Images/test.png")

    #print(type(frame))

    # スクリーンショットを撮りたい関係で1/3サイズに縮小
    #frame = cv2.resize(frame, (int(frame.shape[1]/3), int(frame.shape[0]/3)))

    # ArUcoの処理
    corners, ids, rejectedImgPoints = cv2.aruco.detectMarkers(frame, dictionary)

    #if ids.all()!=None and ids.size==5 and all(ids<=5):
    '''
    if 1 == 1:
        moments = calcMoments(corners,ids)
        marker_coordinates = np.float32(moments[:4])
        trans_mat = cv2.getPerspectiveTransform(marker_coordinates,true_coordinates)
        target_pos = moments[4]
        trans_pos = transPos(trans_mat,target_pos)

        print(str(trans_pos[0]) + " : " + str(trans_pos[1]))
    '''

    frame = cv2.aruco.drawDetectedMarkers(frame, corners, ids)
    if(len(corners) != 0):
        #print(str(corners[0]) + " : " + str(ids[0]))
        sendmsg = ""

        #CriterionMarkersCoordinate = [][]

        for i in range(len(ids)):
            if(ids[i] in CriterionMarkers):
                sendmsg += CreateMessage(False,"CM",ids[i],corners[i])
            if(ids[i] in RoadMarkers):
                sendmsg += CreateMessage(False,"RO",ids[i],corners[i])
            if(ids[i] in BuildingMarkers):
                sendmsg += CreateMessage(False,"BU",ids[i],corners[i])
            if(ids[i] in CleanerMarkers):
                sendmsg += CreateMessage(False,"CL",ids[i],corners[i])

        sendmsg = sendmsg.replace("[","").replace("]","").replace("(","").replace(")","").replace(" ","")

        if(sendmsg != ""):
            print(sendmsg)
            Send(sendmsg)
        #Send(str(corners[0][0]) + " : " + str(ids[0]))
        #print("")

    #print

    # 加工済の画像を表示する
    cv2.imshow('Edited Frame', frame)

    # キー入力を1ms待って、k が27（ESC）だったらBreakする
    k = cv2.waitKey(1)
    if k == 27:
        break

cap.release()
cv2.destroyAllWindows()
