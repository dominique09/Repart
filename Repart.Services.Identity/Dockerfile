FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 53632
EXPOSE 44364

FROM microsoft/dotnet:2.1-sdk-nanoserver-1709 AS build
WORKDIR /src
COPY Repart.Services.Identity/Repart.Services.Identity.csproj Repart.Services.Identity/
COPY Repart.Common/Repart.Common.csproj Repart.Common/
RUN dotnet restore Repart.Services.Identity/Repart.Services.Identity.csproj
COPY . .
WORKDIR /src/Repart.Services.Identity
RUN dotnet build Repart.Services.Identity.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Repart.Services.Identity.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Repart.Services.Identity.dll"]
