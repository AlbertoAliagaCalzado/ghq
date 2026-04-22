FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

WORKDIR "/src/NotifyHub.Presentation"
RUN dotnet restore "NotifyHub.Presentation.csproj"
RUN dotnet publish "NotifyHub.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080 

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NotifyHub.Presentation.dll"]