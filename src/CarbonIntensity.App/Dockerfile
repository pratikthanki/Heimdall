# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

# install NodeJS 14.x, see https://github.com/nodesource/distributions/blob/master/README.md#deb
RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install -y nodejs

COPY src/CarbonIntensity.App/CarbonIntensity.App.csproj .
RUN dotnet restore CarbonIntensity.App.csproj
COPY src/CarbonIntensity.App/ .

RUN dotnet publish -c Release -o /app/publish CarbonIntensity.App.csproj

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CarbonIntensity.App.dll"]