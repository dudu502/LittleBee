var http = require('http');
var url = require('url');
var events = require('events');
var querystring = require("querystring");
var log4js = require('log4js');
log4js.configure({
    replaceConsole: true,
    appenders: {
        cheese: {
            // 设置类型为 dateFile
            type: 'dateFile',
            // 配置文件名为 myLog.log
            filename: 'logs/all_gs_rs.log',
            // 指定编码格式为 utf-8
            encoding: 'utf-8',
            // 配置 layout，此处使用自定义模式 pattern
            layout: {
                type: "pattern",
                // 配置模式，下面会有介绍
                pattern: '"host":"%h","content":\n\'%m\''
            },
            // 日志文件按日期（天）切割
            pattern: "-yyyy-MM-dd",
            // 回滚旧的日志文件时，保证以 .log 结尾 （只有在 alwaysIncludePattern 为 false 生效）
            keepFileExt: true,
            // 输出的日志文件名是都始终包含 pattern 日期结尾
            alwaysIncludePattern: true,
        },
    },
    categories: {
        // 设置默认的 categories
        default: {appenders: ['cheese'], level: 'debug'},
    }
});
var logger = log4js.getLogger('gr_runtime');
var evt = new events.EventEmitter();

http.createServer(function (request, response) {
    var urldata = url.parse(request.url);
    var pathname = urldata.pathname;
    var arr = pathname.split('\'');
    console.log(JSON.stringify(request.connection.remoteAddress));
    console.log("request_id:" + arr[0] + " " + "param:" + urldata.query);
    response.writeHead(200, { 'Content-Type': 'application/json; charset=utf-8' });   
    var request_jsondata = null;
    if (urldata.query != null) {
        request_jsondata = JSON.parse(JSON.stringify(querystring.parse(urldata.query)));      
    }
    evt.emit(arr[0], request_jsondata, response);  
}).listen(8001);
console.log('Server running at port 8001');

function reply_json_data_to_client(response, json_data)
{
    json_data.state = 200;
    response.end(JSON.stringify(json_data));
}
evt.on('/log',function(request_json,response)
{
    logger.info(request_json.content);
    reply_json_data_to_client(response, {});
})
evt.on('/favicon.ico', function (request_json, response){
    reply_json_data_to_client(response, {});
})
