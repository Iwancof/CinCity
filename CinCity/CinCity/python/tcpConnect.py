import socket

def Send(msg):
    msg = str(msg)
    host = "127.0.0.1"
    port = 51081
    client = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
    client.connect((host,port))
    client.send(msg.encode())

def main():
    import time
    for k in range(100):
        Send(k ** 2)
        time.sleep(0.01)

if(__name__ == "__main__"):
    main()