import socket   #for sockets
import sys  #for exit
from datetime import datetime
import signal

host = '192.168.1.81';
port = 10001;
topic = "Topic1"

def run_program():
    
    command = "Subscribe"
    refresh = "10"
    
    try :
        msg = command + "," + topic +"," + refresh
        print msg
        
        #Set the whole string
        s.sendto(msg, (host, port))
        
        
        count = 8 # less than refresh
        event = 0
        while(1) :
            count -=1
            event +=1
        
            # receive data from client (data, addr)
            d = s.recvfrom(1024)
            reply = d[0]
            
            print str(event) +"/"+ str(count) + " " +reply 
            
            #refresh if needed
            if count == 0 :
                count = 8
                command = "Refresh"
                msg = command + "," + topic + "," + refresh
                print msg
                
                #Set the whole string
                s.sendto(msg, (host, port))
        #endWhile
    except socket.error:
        print 'Failed to receive/refresh'
        sys.exit()

def Exit_gracefully(signal, frame):
    command = "UnSubscribe"
    msg = command + "," + topic
    print
    print msg
    
    try: 
        #Set the whole string
        s.sendto(msg, (host, port))
        
    except socket.error, msg:
        print 'Failed to UnSubscribe'
        sys.exit()
        
    s.close()
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
    