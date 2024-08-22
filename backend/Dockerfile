# Use the .NET 6 SDK image as the build environment
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

# Copy csproj and restore as distinct layers

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ./Birdboard.API/Birdboard.API.csproj ./Birdboard.API/
RUN dotnet restore ./Birdboard.API/Birdboard.API.csproj --disable-parallel
COPY . .
WORKDIR /src/Birdboard.API

RUN dotnet build ./Birdboard.API.csproj -c release -o /app/build

FROM build as publish
RUN dotnet publish ./Birdboard.API.csproj -c release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Birdboard.API.dll"]
