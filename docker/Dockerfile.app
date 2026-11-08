FROM mcr.microsoft.com/dotnet/sdk:10.0 AS publish

WORKDIR /src
COPY ./src/AuthenticationProviderApp/AuthenticationProviderApp.csproj .

RUN dotnet restore AuthenticationProviderApp.csproj

COPY ./src/AuthenticationProviderApp .

RUN dotnet publish ./AuthenticationProviderApp.csproj --no-restore -c Release -o /publish
#######################
FROM mcr.microsoft.com/dotnet/aspnet:10.0
RUN apt update && apt-get install -y curl

EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Release
WORKDIR /publish

COPY --from=publish /publish .

ENTRYPOINT [ "dotnet", "./AuthenticationProviderApp.dll"]
