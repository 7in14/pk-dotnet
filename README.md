# pk-dotnet [![Build Status](https://travis-ci.org/7in14/pk-dotnet.svg?branch=master)](https://travis-ci.org/7in14/pk-dotnet)
Sample API created with dotnet core

# dotnet

## create project
project created with `dotnet new webapi`

## build project
`dotnet build`

## run project
Should be `dotnet run` but there's an issue with `args`, apparently the param that OSX is providing is not ok.
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

### Run in Development
```
$ export ASPNETCORE_ENVIRONMENT=Development
$ dotnet run
```

### Run in Production
```
$ export ASPNETCORE_ENVIRONMENT=Production
$ dotnet run
```

## run project with watch
https://docs.microsoft.com/en-us/aspnet/core/tutorials/dotnet-watch

MS went back and forth with `project.json` vs. `*.csproj`. Looks like we're back with `*.csproj`. Most tutorials still refer to `project.json`.
To install `dotnet watch`:
* add `<DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="2.0.0" />` in `*.csproj` file (preferably tools project)
* run `./tools/coverage`

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

## Todo
* Use http://tmenier.github.io/Flurl/testable-http/ for HTTP calls and testing
* Add middleware for request/response logging https://gist.github.com/dazinator/98b2cf4c99e7182b1e11fc4695cadaed or https://blog.getseq.net/smart-logging-middleware-for-asp-net-core/
 * and use https://github.com/tomakita/Colorful.Console
* setup code coverage https://github.com/lucaslorentz/minicover
* use moq for mocking https://github.com/Moq/moq4
