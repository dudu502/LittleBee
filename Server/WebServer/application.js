var http = require('http');
var url = require('url');
var events = require('events');
var querystring = require("querystring");
var evt = new events.EventEmitter();
var mongo_client = require("mongodb").MongoClient;
var mongo_url = "mongodb://localhost:27017/";
var galaxy_database = null;
mongo_client.connect(mongo_url, { useNewUrlParser: true, useUnifiedTopology: true }, function (err, db) {
    if (err) throw err;
    galaxy_database = db.db("galaxy");
    console.log("Connect galaxy database success at " + mongo_url);
});
http.createServer(function (request, response) {
    var urldata = url.parse(request.url);
    var pathname = urldata.pathname;
    var arr = pathname.split('\'');
    //console.log(JSON.stringify(request.connection.remoteAddress));
    //console.log("request_id:" + arr[0] + " " + "param:" + urldata.query);
    response.writeHead(200, { 'Content-Type': 'application/json; charset=utf-8' });   
    var request_jsondata = null;
    if (urldata.query != null) {
        request_jsondata = JSON.parse(JSON.stringify(querystring.parse(urldata.query)));      
    }
    evt.emit(arr[0], request_jsondata, response);  
}).listen(8000);
console.log('Server running at port 8000');

function reply_json_data_to_client(response, json_data)
{
    json_data.state = 200;
    response.end(JSON.stringify(json_data));
}

evt.on('/favicon.ico', function (request_json, response){
    reply_json_data_to_client(response, {});
})
evt.on('/get_index', function (request_json, response)
{
    var response_json = {};
    //response_json.gs_addr = "127.0.0.1";
    response_json.gs_addr = "10.106.19.107";
    response_json.gs_port = "9030";
    response_json.gs_connect_key = "Nuclear";
    reply_json_data_to_client(response, response_json);
})

//  localhost:8000/login?name=tom&password=**
evt.on('/login', function (request_json, response)
{
    galaxy_database.collection("users").find(request_json).toArray(function (err, result) {
        if (err) throw err;
        var response_json = {};
        response_json.results = result;
        reply_json_data_to_client(response,response_json);
    });
});

//  localhost:8000/get_user_info?name=Tom
evt.on('/get_user_info', function (request_json, response)
{
    galaxy_database.collection("users").find(request_json).toArray(function (err, result) {
        if (err) throw err;
        var response_json = {};
        response_json.results = result;
        reply_json_data_to_client(response,response_json);
    });
});

// localhost:8000/contain_user_by_name?name=Tom   
evt.on('/contain_user_by_name',function(request_json,response)
{
    galaxy_database.collection("users").findOne(request_json,function(err,doc)
    {
        if(err)throw err;
        reply_json_data_to_client(response,doc!=null);      //return true or false; 
    });
});

//  localhost:8000/set_user_info?name=tom&pwd=**&infoname0=value&infoname1=value2
evt.on('/insert_user_info', function (request_json, response)
{   
    galaxy_database.collection("users").insertOne(request_json,function(err,result)
    {
        if(err)throw err;
        var response_json = {};
        reply_json_data_to_client(response,response_json);
    });
});

//remove all relation datas;
evt.on('/remove_user_info_by_name',function(request_json,response)
{
    galaxy_database.collection("users").remove(request_json,function(err,result)
    {
        if(err)throw err;
        var query = {};
        query.username = request_json.name;
        galaxy_database.collection("items").remove(query);
        reply_json_data_to_client(response,true);
    });
});
evt.on('/remove_item_by_itemId',function(request_json,response)
{
    galaxy_database.collection("items").remove(request_json,function(err,result)
    {
        if(err)throw err;
        reply_json_data_to_client(response,true);
    });
});


evt.on('/get_items_by_username',function(request_json,response)
{
    galaxy_database.collection("items").find(request_json).toArray(function(err,result)
    {
        if(err)throw err;
        var response_json={};
        response_json.result = result;
        reply_json_data_to_client(response,response_json);
    });
});

evt.on('/insert_item_info',function(request_json,response)
{
    galaxy_database.collection("items").insertOne(request_json,function(err,result)
    {
        if(err)throw err;
        response_json={};
        reply_json_data_to_client(response,response_json);
    });
});

// localhost:8000/update_user_money?name=Tom&money=10000
evt.on('/update_user_money',function(request_json,response)
{
    var query = {};
    query.name = request_json.name;
    var set_new = {};
    set_new.$set = {};
    set_new.$set.money = request_json.money;
    galaxy_database.collection("users").updateOne(query,set_new);    
    reply_json_data_to_client(response,request_json.money);
});
//  localhost:8000/math_add?x=1&y=10
evt.on('/math_add', function (request_json, response)
{  
    var response_json = {};
    response_json.result = parseInt(request_json.x) + parseInt(request_json.y);
    response_json.request = 'test';
    reply_json_data_to_client(response, response_json);
});