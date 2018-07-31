FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 52854
EXPOSE 44312

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY Repart.Services.Formations/Repart.Services.Formations.csproj Repart.Services.Formations/
COPY Repart.Common/Repart.Common.csproj Repart.Common/
RUN dotnet restore Repart.Services.Formations/Repart.Services.Formations.csproj
COPY . .
WORKDIR /src/Repart.Services.Formations
RUN dotnet build Repart.Services.Formations.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Repart.Services.Formations.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Repart.Services.Formations.dll"]
