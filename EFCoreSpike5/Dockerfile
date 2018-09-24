FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Spike.Core.csproj", "EFCoreSpike5/"]
COPY ["ContextFactory.cs", "EFCoreSpike5/"]
WORKDIR "/src/EFCoreSpike5"
COPY ["Entity", "Entity"]
COPY ["Migrations", "Migrations"]
COPY ["Model", "Model"]
#RUN dotnet restore "Spike.Core.csproj"
#COPY . .
#WORKDIR "/src/EFCoreSpike5"
#RUN dotnet build "Spike.Core.csproj" -c Release -o /app

#FROM build AS publish
#RUN dotnet publish "Spike.Core.csproj" -c Release -o /app
ENTRYPOINT ["dotnet", "ef", "database", "update"]

# FROM microsoft/mssql-server-linux
# VOLUME F:\database
# ENV sa_password Password123!
# COPY --from=publish /src .
# ENTRYPOINT ["dotnet", "ef"]