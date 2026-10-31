FROM mcr.microsoft.com/dotnet/sdk:10.0 as publish

WORKDIR /src
COPY ./src/AuthenticationProviderApp/AuthenticationProviderApp.csproj .

RUN dotnet restore AuthenticationProviderApp.csproj

COPY ./src/AuthenticationProviderApp .

RUN dotnet publish ./AuthenticationProviderApp.csproj --no-restore -c Release -o /publish


FROM mcr.microsoft.com/dotnet/aspnet:10.0

EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Release
WORKDIR /publish

COPY --from=build /publish .

ENTRYPOINT [ "dotnet", "./AuthenticationProviderApp.dll"]
