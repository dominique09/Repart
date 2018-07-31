FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 49782
EXPOSE 44307

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY ../Repart.Services.Intervenants/Repart.Services.Intervenants.csproj ../Repart.Services.Intervenants/
RUN dotnet restore ../Repart.Services.Intervenants/Repart.Services.Intervenants.csproj
COPY . .
WORKDIR /src/../Repart.Services.Intervenants
RUN dotnet build Repart.Services.Intervenants.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Repart.Services.Intervenants.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Repart.Services.Intervenants.dll"]
