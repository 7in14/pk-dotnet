# pk-dotnet [![Build Status](https://travis-ci.org/7in14/pk-dotnet.svg?branch=master)](https://travis-ci.org/7in14/pk-dotnet)
Sample API created with dotnet core

# dotnet

## create project
project created with `dotnet new webapi`

## build project
`dotnet build pk-dotnet.sln`

## run project
Should be `dotnet run pk-dotnet.sln` but there's an issue with `args`, apparently the param that OSX is providing is not ok.
```
Unhandled Exception: System.FormatException: Unrecognized argument format: 'pk-dotnet.sln'.
   at Microsoft.Extensions.Configuration.CommandLine.CommandLineConfigurationProvider.Load()
```
this seems to fix it temporarily:
```
    public static void Main(string[] args)
    {
        BuildWebHost(new string[]{}).Run();
    }
```

## run project with watch
https://docs.microsoft.com/en-us/aspnet/core/tutorials/dotnet-watch

MS went back and forth with `project.json` vs. *.csproj. Looks like we're back with *.csproj. Most tutorials still refer to `project.json`.
To install `dotnet watch`:
* add `<DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="2.0.0" />` in *.csproj file
* `cd webapp`
* `dotnet watch run` or `dotnet watch test`

# Docker
https://docs.microsoft.com/en-us/dotnet/core/docker/building-net-docker-images

# Build
run `docker_build.sh`
In theory that builds a production ready image with only runtime on it.


# Run
run `docker_run.sh`

# API
## file
```
$ curl .:8080/api/ping
```

## ping
```
$ curl .:8080/api/ping
```

## Read all data sources
```
$ curl .:8080/api/dataSources
```

## Read one data source
```
$ curl .:8080/api/dataSource/[guid]
```

## Delete data source
```
$ curl -X 'DELETE' .:8080/api/dataSource/[guid]
```
## Add data source
```
$ curl -X 'PUT' .:8080/api/dataSource -d '{"name":"new", "url":"http://google.com"}' -H 'content-type: application/json'
```

## Get crimes
```
curl ".:8080/api/raleigh/crime?query=Drug"
```

## Get all data - from data sources
```
curl .:8080/api/allData
```

## License

MIT