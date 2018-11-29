FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Spike.WebApi.Controllers.Tests/Spike.WebApi.Controllers.Tests.csproj", "Spike.WebApi.Controllers.Tests/"]
COPY ["SpikeWebAPI/Spike.WebApi.csproj", "SpikeWebAPI/"]
COPY ["Spike.Core.Interface/Spike.Core.Interface.csproj", "Spike.Core.Interface/"]
COPY ["EFCoreSpike5/Spike.Core.csproj", "EFCoreSpike5/"]
COPY ["SpikeRepo/Spike.Infrastructure.csproj", "SpikeRepo/"]
COPY ["Spike.WebApi.Types/Spike.WebApi.Types.csproj", "Spike.WebApi.Types/"]
COPY ["Spike.AuthenticationServer.IdentityServer.Types/Spike.AuthenticationServer.IdentityServer.Types.csproj", "Spike.AuthenticationServer.IdentityServer.Types/"]
RUN dotnet restore "Spike.WebApi.Controllers.Tests/Spike.WebApi.Controllers.Tests.csproj"
COPY . .
WORKDIR /src/Spike.WebApi.Controllers.Tests
ENTRYPOINT dotnet test