FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["SpikeWebAPI/Spike.WebApi.csproj", "SpikeWebAPI/"]
COPY ["Spike.Core.Interface/Spike.Core.Interface.csproj", "Spike.Core.Interface/"]
COPY ["EFCoreSpike5/Spike.Core.csproj", "EFCoreSpike5/"]
COPY ["SpikeRepo/Spike.Infrastructure.csproj", "SpikeRepo/"]
COPY ["Spike.WebApi.Types/Spike.WebApi.Types.csproj", "Spike.WebApi.Types/"]
COPY ["Spike.AuthenticationServer.IdentityServer.Types/Spike.AuthenticationServer.IdentityServer.Types.csproj", "Spike.AuthenticationServer.IdentityServer.Types/"]
RUN dotnet restore "SpikeWebAPI/Spike.WebApi.csproj"
COPY . .
WORKDIR "/src/SpikeWebAPI"
RUN dotnet build "Spike.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Spike.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY ["SpikeWebAPI/autofac.json", "."]
ENTRYPOINT ["dotnet", "Spike.WebApi.dll"]