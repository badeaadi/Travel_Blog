FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
EXPOSE 80 8080
COPY . /app

ENV ASPNETCORE_URLS=http://+:8080;http://+:80
USER root

ENTRYPOINT ["dotnet", "Identity.Startup.dll"]
