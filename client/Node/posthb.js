var querystring = require('querystring'),
  http = require('http'),
  apiHost = 'kichline-heartbeat.azurewebsites.net',
  post_data = "{'group': 'Kichline.Kirkland','device': 'Surface','service': 'posthb','status': '###'}";

function PostBeat(statusMsg) {
    post_data = post_data.replace("###", statusMsg);
    var post_options = {
        host: apiHost,
        path: '/api/HeartBeat',
        port: 80,
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Content-Length': post_data.length
        }
    };
    var post_req = http.request(post_options, function (res) {
        res.setEncoding('utf8');
        res.on('data', function (chunk) {
            console.log('Response: ' + chunk);
        });
    });

    post_req.write(post_data);
    post_req.end();
}

PostBeat("OK");
