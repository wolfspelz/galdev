FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet publish code/galdevweb/GaldevWeb/GaldevWeb.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app/out . 
COPY data /data
RUN apt-get update && apt-get install -y libfontconfig1
EXPOSE 80
ENV ASPNETCORE_HTTP_PORTS=80
ENTRYPOINT ["dotnet", "GaldevWeb.dll"]
