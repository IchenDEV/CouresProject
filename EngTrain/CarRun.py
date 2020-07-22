# -*- coding:UTF-8 -*-
import RPi.GPIO as GPIO
import time

Run_speed = 20
Fast_speed = 50
Slow_speed = 5

RTTurnTime =0.21*2
TurnSpeed = 40

buzzer_pin = 8

q = 1.06  # 每阶音的倍数
q2 = q * q
dolist = {'C': 523, 'D': 587, 'E': 659, 'F': 698, 'G': 784, 'A': 880, 'B': 988}
pitchs = {'l': 0.5, 'm': 1, 'h': 2}
# 小车电机引脚定义
IN1 = 20
IN2 = 21
IN3 = 19
IN4 = 26
ENA = 16
ENB = 13

# 小车按键定义
key = 8
#灭火电机引脚设置
OutfirePin = 2

buzzer_pin = 8

# 超声波引脚定义
EchoPin = 0
TrigPin = 1

# RGB三色灯引脚定义
LED_R = 22
LED_G = 27
LED_B = 24

# 舵机引脚定义
ServoPin = 23
# 舵机引脚定义
ServoPin2 = 11
# 舵机引脚定义
ServoPin3 = 9
# 跟随模块引脚定义
FollowSensorLeft = 12
FollowSensorRight = 17
# 红外避障引脚定义
AvoidSensorLeft = 12
AvoidSensorRight = 17

# 循迹红外引脚定义
# TrackSensorLeftPin1 TrackSensorLeftPin2 TrackSensorRightPin1 TrackSensorRightPin2
#      3                 5                  4                   18
TrackSensorLeftPin1 = 3  # 定义左边第一个循迹红外传感器引脚为3口
TrackSensorLeftPin2 = 5  # 定义左边第二个循迹红外传感器引脚为5口
TrackSensorRightPin1 = 4  # 定义右边第一个循迹红外传感器引脚为4口
TrackSensorRightPin2 = 18  # 定义右边第二个循迹红外传感器引脚为18口


# 电机引脚初始化为输出模式
# 按键引脚初始化为输入模式
# 超声波,RGB三色灯,舵机引脚初始化
# 红外避障引脚初始化
# 电机引脚初始化为输出模式
# 按键引脚初始化为输入模式
# 寻迹引脚初始化为输入模式
class Car:
    carLED=True
    def __init__(self):
        global pwm_ENA, pwm_ENB, pwm_servo, pwm_servo2
        # 设置GPIO口为BCM编码方式
        GPIO.setmode(GPIO.BCM)
        # 忽略警告信息
        GPIO.setwarnings(False)
        GPIO.setup(ENA, GPIO.OUT, initial=GPIO.HIGH)
        GPIO.setup(IN1, GPIO.OUT, initial=GPIO.LOW)
        GPIO.setup(IN2, GPIO.OUT, initial=GPIO.LOW)
        GPIO.setup(ENB, GPIO.OUT, initial=GPIO.HIGH)
        GPIO.setup(IN3, GPIO.OUT, initial=GPIO.LOW)
        GPIO.setup(IN4, GPIO.OUT, initial=GPIO.LOW)
        GPIO.setup(buzzer_pin, GPIO.OUT)
        GPIO.setup(TrackSensorLeftPin1, GPIO.IN)
        GPIO.setup(TrackSensorLeftPin2, GPIO.IN)
        GPIO.setup(TrackSensorRightPin1, GPIO.IN)
        GPIO.setup(TrackSensorRightPin2, GPIO.IN)
        GPIO.setup(FollowSensorLeft, GPIO.IN)
        GPIO.setup(FollowSensorRight, GPIO.IN)
        GPIO.setup(OutfirePin,GPIO.OUT,initial=GPIO.HIGH)
        GPIO.setup(EchoPin, GPIO.IN)
        GPIO.setup(TrigPin, GPIO.OUT)
        GPIO.setup(LED_R, GPIO.OUT)
        GPIO.setup(LED_G, GPIO.OUT)
        GPIO.setup(LED_B, GPIO.OUT)
        GPIO.setup(ServoPin, GPIO.OUT)
        GPIO.setup(ServoPin2, GPIO.OUT)
        GPIO.setup(AvoidSensorLeft, GPIO.IN)
        GPIO.setup(AvoidSensorRight, GPIO.IN)
        GPIO.output(buzzer_pin, True)
        # 设置pwm引脚和频率为2000hz
        pwm_ENA = GPIO.PWM(ENA, 2000)
        pwm_ENB = GPIO.PWM(ENB, 2000)
        pwm_ENA.start(0)
        pwm_ENB.start(0)
        # 设置舵机的频率和起始占空比
        pwm_servo = GPIO.PWM(ServoPin, 50)
        pwm_servo.start(0)
        pwm_servo2 = GPIO.PWM(ServoPin2, 50)
        pwm_servo2.start(0)

    def led(self):
        while self.carLED:
            GPIO.setmode(GPIO.BCM)
            GPIO.output(LED_R, GPIO.HIGH)
            GPIO.output(LED_G, GPIO.LOW)
            GPIO.output(LED_B, GPIO.LOW)
            time.sleep(0.1)
            GPIO.output(LED_R, GPIO.LOW)
            GPIO.output(LED_G, GPIO.HIGH)
            GPIO.output(LED_B, GPIO.LOW)
            time.sleep(0.1)
            GPIO.output(LED_R, GPIO.HIGH)
            GPIO.output(LED_G, GPIO.HIGH)
            GPIO.output(LED_B, GPIO.LOW)
            time.sleep(0.1)
    def wind(self,tt):
        GPIO.output(OutfirePin,not GPIO.input(OutfirePin))
        time.sleep(tt)
        GPIO.output(OutfirePin,not GPIO.input(OutfirePin))

    # 小车前进
    def run(self, leftspeed, rightspeed):
        GPIO.output(IN1, GPIO.HIGH)
        GPIO.output(IN2, GPIO.LOW)
        GPIO.output(IN3, GPIO.HIGH)
        GPIO.output(IN4, GPIO.LOW)
        pwm_ENA.ChangeDutyCycle(leftspeed)
        pwm_ENB.ChangeDutyCycle(rightspeed)

    # 小车后退
    def back(self, leftspeed, rightspeed):
        GPIO.output(IN1, GPIO.LOW)
        GPIO.output(IN2, GPIO.HIGH)
        GPIO.output(IN3, GPIO.LOW)
        GPIO.output(IN4, GPIO.HIGH)
        pwm_ENA.ChangeDutyCycle(leftspeed)
        pwm_ENB.ChangeDutyCycle(rightspeed)

    # 小车左转
    def left(self, leftspeed, rightspeed):
        GPIO.output(IN1, GPIO.LOW)
        GPIO.output(IN2, GPIO.LOW)
        GPIO.output(IN3, GPIO.HIGH)
        GPIO.output(IN4, GPIO.LOW)
        pwm_ENA.ChangeDutyCycle(leftspeed)
        pwm_ENB.ChangeDutyCycle(rightspeed)

    # 小车右转
    def right(self, leftspeed, rightspeed):
        GPIO.output(IN1, GPIO.HIGH)
        GPIO.output(IN2, GPIO.LOW)
        GPIO.output(IN3, GPIO.LOW)
        GPIO.output(IN4, GPIO.LOW)
        pwm_ENA.ChangeDutyCycle(leftspeed)
        pwm_ENB.ChangeDutyCycle(rightspeed)

    # 小车原地左转
    def spin_left(self, leftspeed, rightspeed):
        GPIO.output(IN1, GPIO.LOW)
        GPIO.output(IN2, GPIO.HIGH)
        GPIO.output(IN3, GPIO.HIGH)
        GPIO.output(IN4, GPIO.LOW)
        pwm_ENA.ChangeDutyCycle(leftspeed)
        pwm_ENB.ChangeDutyCycle(rightspeed)

    # 小车原地右转
    def spin_right(self, leftspeed, rightspeed):
        GPIO.output(IN1, GPIO.HIGH)
        GPIO.output(IN2, GPIO.LOW)
        GPIO.output(IN3, GPIO.LOW)
        GPIO.output(IN4, GPIO.HIGH)
        pwm_ENA.ChangeDutyCycle(leftspeed)
        pwm_ENB.ChangeDutyCycle(rightspeed)

    # 小车停止
    def brake(self):
        GPIO.output(IN1, GPIO.LOW)
        GPIO.output(IN2, GPIO.LOW)
        GPIO.output(IN3, GPIO.LOW)
        GPIO.output(IN4, GPIO.LOW)

    # 超声波函数
    def Distance_test(self):
        GPIO.output(TrigPin, GPIO.HIGH)
        time.sleep(0.000015)
        GPIO.output(TrigPin, GPIO.LOW)
        while not GPIO.input(EchoPin):
            pass
        t1 = time.time()
        while GPIO.input(EchoPin):
            pass
        t2 = time.time()
        print("distance is %d " % (((t2 - t1) * 340 / 2) * 100))
        time.sleep(0.01)
        return ((t2 - t1) * 340 / 2) * 100

    # 舵机旋转到指定角度
    def servo_appointed_detection(self,pos):
        for i in range(18):
            pwm_servo.ChangeDutyCycle(2.5 + 10 * pos / 180)
    # 舵机旋转到指定角度
    def servo2_appointed_detection(self,pos):
        for i in range(18):
            pwm_servo2.ChangeDutyCycle(2.5 + 10 * pos / 180)
    def go_a_block(self,len):
        self.run(20, 20)
        time.sleep(len)
        self.brake()

    # 舵机旋转超声波测距避障，led根据车的状态显示相应的颜色
    def servo_color_carstate(self):
        while True:
            # 开红灯
            GPIO.output(LED_R, GPIO.HIGH)
            GPIO.output(LED_G, GPIO.LOW)
            GPIO.output(LED_B, GPIO.LOW)

            # 舵机旋转到0度，即右侧，测距
            self.servo_appointed_detection(0)
            time.sleep(0.8)
            rightdistance = self.Distance_test()

            # 舵机旋转到180度，即左侧，测距
            self.servo_appointed_detection(180)
            time.sleep(0.8)
            leftdistance = self.Distance_test()

            # 舵机旋转到90度，即前方，测距
            self.servo_appointed_detection(90)
            time.sleep(0.8)
            frontdistance = self.Distance_test()

            if leftdistance < 30 and rightdistance < 30 and frontdistance < 30:
                # 亮品红色，掉头
                GPIO.output(LED_R, GPIO.HIGH)
                GPIO.output(LED_G, GPIO.LOW)
                GPIO.output(LED_B, GPIO.HIGH)
                self.spin_right(TurnSpeed, TurnSpeed)
                time.sleep(RTTurnTime*2)
            elif leftdistance > 50 and rightdistance > 50 and frontdistance > 50:
                break
            elif leftdistance >= rightdistance:
                # 亮蓝色
                GPIO.output(LED_R, GPIO.LOW)
                GPIO.output(LED_G, GPIO.LOW)
                GPIO.output(LED_B, GPIO.HIGH)
                self.spin_left(TurnSpeed, TurnSpeed)
                time.sleep(RTTurnTime)
            elif leftdistance <= rightdistance:
                # 亮品红色，向右转
                GPIO.output(LED_R, GPIO.HIGH)
                GPIO.output(LED_G, GPIO.LOW)
                GPIO.output(LED_B, GPIO.HIGH)
                self.spin_right(TurnSpeed, TurnSpeed)
                time.sleep(RTTurnTime)
            self.go_a_block(1.2)

    def follow(self):
        while True:
            # 遇到跟随物,红外跟随模块的指示灯亮,端口电平为LOW
            # 未遇到跟随物,红外跟随模块的指示灯灭,端口电平为HIGH
            LeftSensorValue = GPIO.input(FollowSensorLeft)
            RightSensorValue = GPIO.input(FollowSensorRight)

            if LeftSensorValue == False and RightSensorValue == False:
                if self.Distance_test() > 8 or self.Distance_test() <=0:
                    self.run(Run_speed, Run_speed)  # 当两侧均检测到跟随物时调用前进函数
                else:
                    self.brake()
                    print('stop')
                    break
            elif LeftSensorValue == False and RightSensorValue == True:
                self.spin_left(45,45)  # 左边探测到有跟随物，有信号返回，原地向左转
                time.sleep(0.002)
            elif RightSensorValue == False and LeftSensorValue == True:
                self.spin_right(45,45)  # 右边探测到有跟随物，有信号返回，原地向右转
                time.sleep(0.002)
            elif RightSensorValue == True and LeftSensorValue == True:
                self.brake()  # 当两侧均未检测到跟随物时停止

    # 按键检测
    def key_scan(self):
        while GPIO.input(key):
            pass
        while not GPIO.input(key):
            time.sleep(0.01)
            if not GPIO.input(key):
                time.sleep(0.01)
            while not GPIO.input(key):
                pass

    def tracking(self):
        break_count = 0
        while True:
            # 检测到黑线时循迹模块相应的指示灯亮，端口电平为LOW
            # 未检测到黑线时循迹模块相应的指示灯灭，端口电平为HIGH
            track_sensor_left_value1 = GPIO.input(TrackSensorLeftPin1)
            track_sensor_left_value2 = GPIO.input(TrackSensorLeftPin2)
            track_sensor_right_value1 = GPIO.input(TrackSensorRightPin1)
            track_sensor_right_value2 = GPIO.input(TrackSensorRightPin2)

            if track_sensor_left_value1 == False and track_sensor_left_value2 == False and track_sensor_right_value2 == False and track_sensor_right_value1 == False:
                self.brake()
                break
            # 四路循迹引脚电平状态
            # 0 0 1 0
            # 1 0 X 0
            # 0 1 X 0
            # 以上6种电平状态时小车原地右转
            # 处理右锐角和右直角的转动
            elif (
                    track_sensor_left_value1 == False or track_sensor_left_value2 == False) and track_sensor_right_value2 == False:
                self.spin_right(25, 25)
                time.sleep(0.04)
            # 四路循迹引脚电平状态
            # 0 X 0 0
            # 0 X 0 1
            # 0 X 1 0
            # 处理左锐角和左直角的转动
            elif track_sensor_left_value1 == False and (
                    track_sensor_right_value1 == False or track_sensor_right_value2 == False):
                self.spin_left(25, 25)
                time.sleep(0.04)
            # 0 X X X
            # 最左边检测到
            elif not track_sensor_left_value1:
                self.spin_left(40, 40)
            # X X X 0
            # 最右边检测到
            elif track_sensor_right_value2 == False:
                self.spin_right(40, 40)
            # 四路循迹引脚电平状态
            # X 0 1 X
            # 处理左小弯
            elif track_sensor_left_value2 == False and track_sensor_right_value1 == True:
                self.left(0, 45)
            # 四路循迹引脚电平状态
            # X 1 0 X
            # 处理右小弯
            elif track_sensor_left_value2 == True and track_sensor_right_value1 == False:
                self.right(45, 0)
            # 四路循迹引脚电平状态
            # X 0 0 X
            # 处理直线
            elif track_sensor_left_value2 == False and track_sensor_right_value1 == False:
                self.run(20, 20)
                time.sleep(0.05)
                #delay？？？
            # 当为1 1 1 1时小车保持上一个小车运行状态

    def BBPlayer(self, filename, dokey, speed):
        do = int(dolist[dokey])  # 获取对应调的do的频率
        re = int(do * q2)
        mi = int(re * q2)
        fa = int(mi * q)
        sol = int(fa * q2)
        la = int(sol * q2)
        si = int(la * q2)
        notes = [0, do, re, mi, fa, sol, la, si]
        beats = 60.0 / speed * 1000
        with open(filename) as fp:
            song = fp.read().replace('\n', '').split(',')
            for music in song:
                p = music[1]
                p = float(pitchs[p])  # 高低音
                n = int(notes[int(music[0])])  # 音符
                b = float(music[2:])  # 节拍
                print(n)
                print(b * beats)
                if self.carLED == False:
                    GPIO.output(buzzer_pin, True)
                    break
                if n == 0:
                    time.sleep(b * beats / 1000)
                else:
                    self.buzz(n, b * beats / 1000)
                    time.sleep(0.5)

    def stop_car_in(self):
        self.spin_left(TurnSpeed, TurnSpeed)
        time.sleep(RTTurnTime)
        self.back(20, 20)
        time.sleep(1)
        self.brake()

    def buzz(self, pitch, duration):
        period = 1.0 / pitch
        delay = period / 2
        cycles = int(duration * pitch)
        for i in range(cycles):
            GPIO.output(buzzer_pin, True)
            time.sleep(delay)
            GPIO.output(buzzer_pin, False)
            time.sleep(delay)

    def clean(self):
        pwm_ENA.stop()
        pwm_ENB.stop()
        GPIO.output(buzzer_pin, True)
        GPIO.cleanup()
    
    def __del__(self):
        self.clean()
        GPIO.output(buzzer_pin, True)