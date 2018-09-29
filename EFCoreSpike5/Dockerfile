FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Spike.Core.csproj", "EFCoreSpike5/"]
COPY ["ContextFactory.cs", "EFCoreSpike5/"]
WORKDIR "/src/EFCoreSpike5"
COPY ["Entity", "Entity"]
COPY ["Migrations", "Migrations"]
COPY ["Model", "Model"]

ENTRYPOINT ["dotnet", "ef", "database", "update"]