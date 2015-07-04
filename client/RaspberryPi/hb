#!/usr/bin/python
import time, httplib

apiHost = 'kichline-heartbeat.azurewebsites.net'
apiPort = 80
apiPath = '/api/heartbeat'
apiQuery= 'group=Kichline.Kirkland&device=RPi2&service=uptime&status='

startTime = time.time()

def hb(stat):
  conn = httplib.HTTPConnection(apiHost, apiPort)
  conn.connect()
  conn.set_debuglevel(0)
  request = conn.putrequest('POST', apiPath+'?'+apiQuery+stat)
  headers = {}
  headers['Content-Length'] = "0"
  headers['Accept'] = '*/*'
  for k in headers:
    conn.putheader(k, headers[k])
  conn.endheaders()
  
  conn.send('')
  resp = conn.getresponse()
  conn.close()

while True:
  interval = int(time.time() - startTime)
  hb(str(interval))
  time.sleep(300)
