var querystring = require('querystring'),
  http = require('http'),
  apiHost = 'kichline-heartbeat.azurewebsites.net',
  post_info = {
    'group': 'Kichline.Kirkland',
    'device': 'Surface',
    'service': 'posthb',
    'status': ''
  };

function PostBeat(statusMsg) {
  post_info.status = statusMsg;
  var post_options = {
    host: apiHost,
    port: '80',
    path: '/api/HeartBeat?' + querystring.stringify(post_info),
    method: 'POST',
  };
  //console.log(post_options.host+post_options.path);
  var post_req = http.request(post_options, function (res) {
    res.setEncoding('utf8');
    res.on('data', function (chunk) {
      console.log('Response: ' + chunk);
    });
  });
  post_req.write("");
  post_req.end();
}

function KeepBeat(statusMsg) {
  PostBeat(statusMsg);
  return setInterval(PostBeat, 300000, statusMsg);
}

KeepBeat("UP");

