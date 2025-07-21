FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY LocationRegionMatcher/*.csproj LocationRegionMatcher/
COPY LocationRegionMatcher.Tests/*.csproj LocationRegionMatcher.Tests/
RUN dotnet restore LocationRegionMatcher/LocationRegionMatcher.csproj

COPY . .
RUN dotnet publish LocationRegionMatcher/LocationRegionMatcher.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "LocationRegionMatcher.dll"]