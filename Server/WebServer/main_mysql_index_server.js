var http = require('http');
var url = require('url');
var events = require('events');
var querystring = require("querystring");
var mysql = require("mysql");
var connection = mysql.createConnection({
host	:	'localhost',
user	:	'root',
password:	'a1+a2=a3',
database:	'galaxydb'
});
connection.connect();
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
    try{
        evt.emit(arr[0], request_jsondata, response);
    }catch(exc){
        console.log(exc);
    }
      
}).listen(3000);
console.log("index server is ready on 3000");
function reply_json_data_to_client(response, json_data)
{
    json_data.state = 200;
    response.end(JSON.stringify(json_data));
}
function reply_error_json_data_to_client(response,json_data){
    json_data.state = -1;
    response.end(JSON.stringify(json_data));
}

// localhost:8000/cmd?sql=select * from tables
evt.on('/cmd',function(request_json,response){
    var sql = request_json.sql;
    connection.query(sql,function(err,result)
        {
            var response_json={};
            if(err){
                response_json.results=[];
                console.log("error "+err.message);
                reply_error_json_data_to_client(response,response_json);
                return;
            }
            var result_json=JSON.parse(JSON.stringify(result));
            response_json.results = result_json;
            reply_json_data_to_client(response,response_json);
        });
});

evt.on('/favicon.ico', function (request_json, response){
    reply_json_data_to_client(response, {});
})
evt.on('/get_index', function (request_json, response)
{
    var response_json = {};
    response_json.gs_addr = "175.24.198.37";
    //response_json.gs_addr = "1.15.139.24";
    //response_json.gs_addr = "127.0.0.1";
    //response_json.gs_addr = "192.168.137.1";
    response_json.gs_port = "9030";
    response_json.gs_connect_key = "Nuclear";
    reply_json_data_to_client(response, response_json);
})

//  localhost:8000/login?name=tom&password=**
evt.on('/login', function (request_json, response)
{
	var sql="select * from user_tbl where name='"+request_json.name+"'";
	connection.query(sql,function(err,result)
    	{
            var response_json = {};
            response_json.results=[];
    		if(err)
    		{
    			console.log("error "+err.message);
                reply_error_json_data_to_client(response,response_json);
    			return;
    		}

    		var result_json=JSON.parse(JSON.stringify(result));

    		if(result_json.length!=0&&result_json[0].pwd==request_json.password)
    		{
    			response_json.results=result_json;
    		}
    		reply_json_data_to_client(response,response_json);		
    	});
});

//  localhost:8000/get_user_info?name=Tom
evt.on('/get_user_info', function (request_json, response)
{
    var sql = "select * from user_tbl where name='"+request_json.name+"'";
    connection.query(sql,function(err,result)
        {
            var response_json = {};
            if(err)
            {
                console.log("error "+err.message);
                reply_error_json_data_to_client(response,response_json);
                return;
            }
            var result_json=JSON.parse(JSON.stringify(result));
            response_json.results = result_json;
            reply_json_data_to_client(response,response_json);
        });
});

evt.on('/list_all_users',function(request_json,response)
{
    var sql = "select * from user_tbl";
    connection.query(sql,function(err,result)
        {
            var response_json={};
            if(err)
            {
                console.log("error "+err.message);
                reply_error_json_data_to_client(response,response_json);
                return;
            }
            var result_json=JSON.parse(JSON.stringify(result));
            response_json.results = result_json;
            reply_json_data_to_client(response,response_json);
        });
});

// localhost:8000/contain_user_by_name?name=Tom   
evt.on('/contain_user_by_name',function(request_json,response)
{
    var sql = "select name from user_tbl where name='"+request_json.name+"'";
    connection.query(sql,function(err,result)
        {
            var response_json={};
            if(err)
            {
                console.log("error "+err.message);
                reply_error_json_data_to_client(response,response_json);
                return;
            }
            var result_json=JSON.parse(JSON.stringify(result));
            response_json.results = result_json;
            reply_json_data_to_client(response,response_json);
        });
});

//  localhost:8000/insert_user_info?name=tom&pwd=**&infoname0=value&infoname1=value2
evt.on('/insert_user_info', function (request_json, response)
{   
    var sql = "insert into user_tbl(name,pwd,date,state,money) values (?,?,?,0,0)";
    var sqlparams = [request_json.name,"1",Date.now()];
   
    connection.query(sql,sqlparams,function(err,result)
    {
        var response_json={};
        if(err)
        {
            console.log("error "+err.message);
            reply_error_json_data_to_client(response,response_json);
            return;
        }
        reply_json_data_to_client(response,response_json);
    });
});


// localhost:8000/update_user_money?name=Tom&money=10000
evt.on('/update_user_money',function(request_json,response)
{
    var sql = "UPDATE user_tbl SET money = ? WHERE name = ?";
    var sqlparams = [request_json.money,request_json.name];
    connection.query(sql,sqlparams,function (err,result) {
        var response_json={};
        if(err)
        {
            reply_error_json_data_to_client(response,response_json);
            return;
        }
        console.log("UPDATE Affected ROWS ",result.affectedRows);
        reply_json_data_to_client(response,response_json);
    });
});

//  localhost:8000/math_add?x=1&y=10
evt.on('/math_add', function (request_json, response)
{  
    var response_json = {};
    response_json.result = parseInt(request_json.x) + parseInt(request_json.y);
    response_json.request = 'test';
    reply_json_data_to_client(response, response_json);
});
