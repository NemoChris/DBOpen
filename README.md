# DBOpen
## introduction
DBOpen is a lightweight framework for connecting database that lets you operate the database more easily.It currently supports SqlServer and MySQL,You can extend it to support more databases.
## Development environment
Visutal Studio version:2013
.netFramework version:4.5
# Getting Start
## Step1 Configuration web.config
First you need to configure web.config to operate specified database.This is illustrated by the following code snippet:    
               
	<configuration>
	<appSettings>
	<!--SqlServer Settings-->
    <!--<add key="Test" value="Data Source=192.168.1.66;Initial Catalog=Test;User ID=sa;Password=0000"/>        
    <add key="DBOpenControllerName" value="Sql"/>-->
    <!--Mysql Settings-->
    <add key="Test" value="server=192.168.1.66;user=root;database=mybatis;password=0000;port=3306;"/>    
    <add key="DBOpenControllerName" value="MySql"/>
	</appSettings>
	</configuration>

Configuration DBOpenControllerName to operate specified database.It's value can only be Sql or MySql.Here we configure SqlServer as a database for our operations.

## Step2 Create SqlController and execute query

Next we need to create a SqlController to operate the database.This is illustrated by the following code snippet:
    
    IController ic = ControllerFactory.CreateController();
    System.Collections.Generic.List<TestDefaultValue> list = ic.Query<TestDefaultValue>("SELECT * FROM [Test_DefaultValue]");

