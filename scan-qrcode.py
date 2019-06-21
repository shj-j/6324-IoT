
#scan QRcode and connect to our online service website 'http://iot-stadium.azurewebsites.net/shop/'
import qrcode
import os
import sys
import time
 
QRImagePath = os.getcwd() + '/qrcode.png'   #template qrcode file location
qr = qrcode.QRCode(     
    version=1,
    error_correction=qrcode.constants.ERROR_CORRECT_L,
    box_size=10,
    border=2,
)   #set format of qrcode
 
#data = input()  #input qrcode data
data = {"Status":1,
        "Flag":0,
        "Bay":12,
        "Row":2,
        "Seat":53,
        }
data = 'http://iot-stadium.azurewebsites.net/shop/'
qr.add_data(data)
qr.make(fit=True)
 
img = qr.make_image()
img.save('qrcode.png')  #generate qrcode
 
if sys.platform.find('darwin') >= 0:
    os.system('open %s' % QRImagePath)
    
elif sys.platform.find('linux') >= 0:
    os.system('xdg-open %s' % QRImagePath)
else:
    os.system('call %s' % QRImagePath)
    
time.sleep(5)   #time sleep
os.remove(QRImagePath)  #delete qrcode
