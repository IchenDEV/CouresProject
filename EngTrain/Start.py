import time
import threading
from TLState import TrLight
from CarRun import Car

# 延时2s
time.sleep(1)

try:
    car1 = Car()

    for i in range(2):
        car1.tracking()
        while TrLight(1) != 'green':#红绿灯
            print('stop')
            time.sleep(1)
        print('go')
        car1.run(10,10)
        time.sleep(0.2)

    car1.tracking()
    car1.go_a_block(0.9)
    car1.servo_color_carstate()#超声波
    car1.go_a_block(0.2)
    time.sleep(3)
    
    ta = threading.Thread(target=car1.led)  # 创建一个线程ta，执行 threadfun()
    tb = threading.Thread(target=car1.BBPlayer, args=('little_star', 'C', 65))  # 创建一个线程tb，执行threadfun()
    tc = threading.Thread(target=car1.follow)  # 创建一个线程tb，执行threadfun()
    ta.start()  # 调用start()，运行线程
    tb.start()  # 调用start()，运行线程
    tc.start()
    tc.join()
    car1.brake()
    car1.carLED=False
    car1.wind(1)
    time.sleep(2)


    car1.tracking()

    while TrLight(1) != 'yellow':#红绿灯
            print('stop')
            time.sleep(1)
    car1.stop_car_in()

    car1.clean()



except KeyboardInterrupt:
    car1 = Car()
    car1.clean()
    pass
