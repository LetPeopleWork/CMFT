FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5001

# Copy the default certificate
COPY ["Lighthouse.Backend/Lighthouse.Backend/certs/LighthouseCert.pfx", "/app/certs/LighthouseCert.pfx"]

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG VERSION=0.0.1
WORKDIR /src
COPY ["Lighthouse.Backend/", "."]
WORKDIR "/src"
RUN dotnet restore "./Lighthouse.sln" -a "$TARGETARCH"
RUN dotnet build "Lighthouse.Backend/Lighthouse.Backend.csproj" -c "$BUILD_CONFIGURATION" -o /app/build/
RUN dotnet build "Lighthouse.Migrations.Postgres/Lighthouse.Migrations.Postgres.csproj" \
	-c "$BUILD_CONFIGURATION" \
	-o /app/build/
RUN dotnet build "Lighthouse.Migrations.Sqlite/Lighthouse.Migrations.Sqlite.csproj" \
	-c "$BUILD_CONFIGURATION" \
	-o /app/build/

FROM node:22 AS node-builder
WORKDIR /node
COPY Lighthouse.Frontend /node
RUN npm install --ignore-scripts \
&& npm run build-docker

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
COPY --from=node-builder /node/dist ./wwwroot
RUN dotnet publish "./Lighthouse.Backend/Lighthouse.Backend.csproj" \
	-c "$BUILD_CONFIGURATION" \
	-o /app/publish \
	/p:UseAppHost=false \
	/p:PublishSingleFile=false \
	/p:Version="$VERSION" \
	--no-self-contained \
	/p:EnablePublishBeforeCleanup=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lighthouse.dll"]