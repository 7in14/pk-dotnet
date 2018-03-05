FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /dotnetapp

# copy csproj and restore as distinct layers
COPY *.sln .
COPY app/*.csproj ./app/
RUN dotnet restore

# copy everything else and build app
COPY app/. ./app/
WORKDIR /dotnetapp/app
RUN dotnet publish -o out /p:PublishWithAspNetCoreTargetManifest="false"


FROM microsoft/dotnet:2.0-runtime AS runtime
ENV ASPNETCORE_URLS http://+:80
WORKDIR /dotnetapp
COPY --from=build /dotnetapp/app/out ./
ENTRYPOINT ["dotnet", "pk-dotnet.dll"]
