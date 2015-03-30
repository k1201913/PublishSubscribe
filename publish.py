import socket   #for sockets
import sys  #for exit
from datetime import datetime
import signal


host = '192.168.1.81';
port = 10002;
command = "Publish"
topic = "Topic1"
 

def run_program():
    while(1) :
        msg = raw_input('Enter message to send (x = quit): ')
        date = datetime.now() 
        if msg == "x" :
            break
        
        dstr = date.strftime("%d.%m.%Y %H:%M:%S")
        msg = command + "," + dstr + "," + topic + "," + msg
        print msg
        
        try :
            s.sendto(msg, (host, port))
            
        except socket.error, msg:
            print 'Failed to Publish ' + msg
            s.close()
            sys.exit()
    s.close()
    print 'closing...'
    
def Exit_gracefully(signal, frame):

    s.close()
    print
    print 'closing...'
    sys.exit()


if __name__ == '__main__':
    signal.signal(signal.SIGINT, Exit_gracefully)
    # create dgram udp socket
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    except socket.error:
        print 'Failed to create socket'
        sys.exit()
    run_program()
    