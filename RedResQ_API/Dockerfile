#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RedResQ_API/RedResQ_API.csproj", "RedResQ_API/"]
COPY ["RedResQ_API.Lib/RedResQ_API.Lib.csproj", "RedResQ_API.Lib/"]
RUN dotnet restore "RedResQ_API/RedResQ_API.csproj"
COPY . .
WORKDIR "/src/RedResQ_API"
RUN dotnet build "RedResQ_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RedResQ_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RedResQ_API.dll"]