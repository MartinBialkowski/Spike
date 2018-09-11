FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 53702
EXPOSE 44307

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Spike.AuthenticationServer.IdentityServer/Spike.AuthenticationServer.IdentityServer.csproj", "Spike.AuthenticationServer.IdentityServer/"]
COPY ["EFCoreSpike5/Spike.Core.csproj", "EFCoreSpike5/"]
COPY ["Spike.AuthenticationServer.IdentityServer.Types/Spike.AuthenticationServer.IdentityServer.Types.csproj", "Spike.AuthenticationServer.IdentityServer.Types/"]
COPY ["SpikeConnectProviders/Spike.Backend.Connect.csproj", "SpikeConnectProviders/"]
COPY ["Infrastructure/Spike.Backend.Interface.csproj", "Infrastructure/"]
RUN dotnet restore "Spike.AuthenticationServer.IdentityServer/Spike.AuthenticationServer.IdentityServer.csproj"
COPY . .
WORKDIR "/src/Spike.AuthenticationServer.IdentityServer"
RUN dotnet build "Spike.AuthenticationServer.IdentityServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Spike.AuthenticationServer.IdentityServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Spike.AuthenticationServer.IdentityServer.dll"]