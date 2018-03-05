# pk-dotnet

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