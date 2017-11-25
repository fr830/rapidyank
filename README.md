# rapidyank
rapidyank is a net core 2 console app that will constantly poll [Rapid Scada](https://github.com/RapidScada)'s web service to get readings and persist them in a relational database.

# Getting Started

## Environment Setup
This project assumes you have a relational database with a table called readings.

Basic MSSQL Example:
```sql
CREATE TABLE reading (
	id          BIGINT IDENTITY(1,1) NOT NULL,
	channel     INT NOT NULL,
	date        DATETIME NOT NULL,
	value       FLOAT NOT NULL,
	status      INT NOT NULL,
	text        NVARCHAR(50) NOT NULL,
	textandunit NVARCHAR(50) NOT NULL,
	color       NVARCHAR(50) NOT NULL)
```

You can run the program with Visual Studio 2017, using net core 2. Modify **appsettings.json** to reflect your environment:
* **BaseUrl** points to your rapid scada server web service
* **Username** Rapid Scada user that has access to the views you want to get readings from
* **Password** Rapid Scada user's password
* **ConnectionString** Connection string to the database you will be persisting the readings to
* **IntervalSeconds** Poll interval in seconds
* **MaxErrors** Number of errors before the app exits with an error exit code
* **Channels** an array of tuples consisting of the channel number and the view number for that channel
* **Views** specify the view number to get all the channels it contains (currently not working)

## Running the app

Test if the web service connection is working
```bash
dotnet rapidyank.dll test --webservice
```

Test if the database connection is working (not implemented yet)
```bash
dotnet rapidyank.dll test --database
```

Start polling data
```bash
dotnet rapidyank.dll transfer
```

# This Project Uses

* [commandline](https://github.com/commandlineparser/commandline) - C# command line parser for .NET
* [Dapper](https://github.com/StackExchange/Dapper) - a simple object mapper for .Net
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) - a popular high-performance JSON
* [NLog](https://github.com/NLog/NLog) - Advanced .NET, .NET Standard, Silverlight and Xamarin Logging
* [RestSharp](https://github.com/restsharp/RestSharp) - Simple REST and HTTP API Client for .NET
