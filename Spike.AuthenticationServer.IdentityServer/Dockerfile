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

ARG VCS_REF
ARG BUILD_VERSION

# Labels
LABEL com.embe-projects.name="AuthService"
LABEL com.embe-projects.description="Image for auth service used for embe's projects"
LABEL com.embe-projects.url="TO DO as blog post or wiki documentation"
LABEL com.embe-projects.vcs-url="https://embeprojects.visualstudio.com/HeroesManager/_git/Spike?path=%2FSpike.AuthenticationServer.IdentityServer&version=GBdevelop"
LABEL com.embe-projects.vcs-ref=$VCS_REF
LABEL com.embe-projects.version=$BUILD_VERSION
LABEL com.embe-projects.version="1.0-test"
LABEL com.embe-projects.schema-version="1.0"
LABEL com.embe-projects.docker.cmd="docker run --rm -p 53702:80 -p 44307:443 spikeauthenticationserveridentityserver --name myapp"
LABEL com.embe-projects.docker.cmd.devel="TODO run in developer mode"
LABEL com.embe-projects.docker.cmd.test="TODO run only tests"
LABEL com.embe-projects.docker.params="TODO environmental parameters"
