import time

import cv2
import numpy as np
from enum import Enum
from detectColor import detectColor


class TLState(Enum):
    others = 0
    red = 1
    yellow = 2
    green = 3
    red_yellowArrow = 4
    red_greenArrow = 5
    green_yellowArrow = 6
    green_greenArrow = 7
    redArrow = 8
    yellowArrow = 9
    greenArrow = 10
    flashingYellowArrow = 11


class TLType(Enum):
    regular = 0
    five_lights = 1
    four_lights = 2


def imgResize(image, height, inter=cv2.INTER_AREA):
    # initialize the dimensions of the image to be resized and grab the image size
    dim = None
    (h, w) = image.shape[:2]
    # calculate the ratio of the height and construct the dimensions
    r = height / float(h)
    dim = (int(w * r), height)
    # resize the image
    resized = cv2.resize(image, dim, interpolation=inter)
    # return the resized image
    return resized


def detectState(image, TLType):
    image = imgResize(image, 300)
    (height, width) = image.shape[:2]
    output = image.copy()
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    # 霍夫圆环检测
    circles = cv2.HoughCircles(gray, cv2.HOUGH_GRADIENT, 1, 20,
                               param1=50, param2=30, minRadius=15, maxRadius=30)
    overallState = 0
    stateArrow = 0
    stateSolid = 0
    if circles is not None:
        circles = np.uint16(np.around(circles))

        for i in circles[0, :]:
            if i[1] < i[2]:
                i[1] = i[2]
            roi = image[(i[1] - i[2]):(i[1] + i[2]), (i[0] - i[2]):(i[0] + i[2])]
            color = detectColor(roi)
            if color > 0:
                if TLType == 1 and i[0] < width / 2 and i[1] > height / 3:
                    stateArrow = color
                elif TLType == 2:
                    stateArrow = color
                    if i[1] > height / 2 and i[1] < height / 4 * 3:
                        stateArrow = color + 2
                else:
                    stateSolid = color

    if TLType == 1:
        overallState = stateArrow + stateSolid + 1
    elif TLType == 2:
        overallState = stateArrow + 7
    else:
        overallState = stateSolid

    return overallState


def plot_light_result1(image):
    img = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
    label = TLState(detectState(image, TLType.regular.value)).name
    print(label)
    return label


def TrLight(camx):
    global red, yellow, green, others
    red = 0
    yellow = 0
    green = 0
    others = 0
    cap = cv2.VideoCapture(camx)
    if not cap.isOpened():
        cap.open(camx)
    print(cap.isOpened())
    while cap.isOpened():
        ok, frame = cap.read()  # 读取一帧数据
        if not ok:
            break
        label = plot_light_result1(frame)
        if label == 'green':
            green = green + 1
        elif label == 'yellow':
            yellow = yellow + 1
        elif label == 'red':
            red = red + 1
        elif label == 'others':
            others = others + 1
        if green > 5:
            red = 0
            green = 0
            yellow = 0
            others = 0
            cap.release()
            return 'green'
        elif red > 5:
            red = 0
            green = 0
            yellow = 0
            others = 0
            cap.release()
            return 'red'
        elif yellow > 5:
            red = 0
            green = 0
            yellow = 0
            others = 0
            cap.release()
            return 'yellow'
        elif others > 5:
            red = 0
            green = 0
            yellow = 0
            others = 0
            cap.release()
            return 'others'
        # cv2.imshow("Camera", frame)
        #c = cv2.waitKey(10)
        time.sleep(0.1)
        #if c & 0xFF == ord('q'):  # 按q退出
            #break
    cap.release()
    #cv2.destroyAllWindows()

