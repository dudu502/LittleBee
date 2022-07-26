var http = require('http');
var url = require('url');
var util = require('util');
 
http.createServer(function(req, res){
    res.writeHead(200, {'Content-Type': 'text/html'});
    res.end('<font size="300" color="red">sbzhy</font>');
}).listen(3000);
console.log('Server running at port 3000');