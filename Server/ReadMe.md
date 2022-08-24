###  Deploy on Ubuntu 18.04
#### Install dotnet 3.1

``` shell
wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb>

sudo dpkg -i packages-microsoft-prod.deb

sudo apt-get update

sudo apt-get install -y apt-transport-https

sudo apt-get update

sudo apt-get install -y dotnet-sdk-3.1
```


#### Install nodejs
``` shell
curl -sL https://deb.nodesource.com/setup_16.x | sudo -E bash -

sudo apt-get install -y nodejs
```
#### Install mysql server
``` shell
sudo apt install mysql-server

sudo chown -R mysql:mysql /var/lib/mysql

mysqld --initialize

systemctl start mysqld   or systemctl start mysql.service

[root@host]# mysqladmin -u root password "new_password";
connect to server
[root@host]# mysql -u root -p
```
#### Install mysql driver for nodejs
``` shell
npm install mysql
```
#### Error: ER_NOT_SUPPORTED_AUTH_MODE: Client does not support authentication protocol requested by server; consider upgrading MySQL client

``` shell
mysql -u root -p
use mysql;
alter user 'root'@'localhost' identified with mysql_native_password by 'a1+a2=a3';
flush privileges;
```
#### Create a database 
``` shell
mysql> create DATABASE galaxydb;
```
#### Create a table user_tbl
``` mysql
mysql> create table user_tbl(
    
    -> name VARCHAR(32) NOT NULL,
    
    -> pwd VARCHAR(32) NOT NULL,
    
    -> money INT,
    
    -> state INT,
    
    -> date VARCHAR(64) NOT NULL,
    
    -> PRIMARY KEY(name)
    
    -> )ENGINE=InnoDB DEFAULT CHARSET=utf8;
```

#### Flow
1.Start web server

2.Start Gate server(config start.json under this project to link the Room server)
