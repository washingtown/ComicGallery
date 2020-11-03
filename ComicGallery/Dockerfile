#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ComicGallery/ComicGallery.csproj", "ComicGallery/"]
RUN dotnet restore "ComicGallery/ComicGallery.csproj"
COPY . .
WORKDIR "/src/ComicGallery"
RUN dotnet build "ComicGallery.csproj" -c Release -o /app/build

FROM build AS publish
RUN mv -f appsettings-docker.json appsettings.json\
    && mkdir -p /data/db\
    && mkdir -p /data/settings\
    && mkdir -p /data/galleries\
    && dotnet publish "ComicGallery.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /data /data
RUN ln /app/appsettings.json /data/settings/appsettings.json
ENTRYPOINT ["dotnet", "ComicGallery.dll","--urls","http://0.0.0.0:80"]