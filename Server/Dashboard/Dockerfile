FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview-alpine AS base
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview-alpine AS build
WORKDIR /workdir
COPY . .
RUN dotnet restore	"./Dashboard/Dashboard.csproj"
RUN dotnet build	"./Dashboard/Dashboard.csproj" -c Release -o ./app/build

FROM build AS publish
RUN dotnet publish "./Dashboard/Dashboard.csproj" -c Release -o ./app/publish /p:UseAppHost=false

FROM base AS production
WORKDIR /workdir
COPY --from=publish /workdir/app/publish .

ENTRYPOINT ["dotnet", "Dashboard.dll"]
