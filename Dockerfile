# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# Set the PORT environment variable
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Use the official ASP.NET Core SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

# Restore and build dependencies
COPY ["CDR.API/CDR.API.csproj", "CDR.API/"]
COPY ["CDR.Data/CDR.Data.csproj", "CDR.Data/"]
COPY ["CDR.Entities/CDR.Entities.csproj", "CDR.Entities/"]
COPY ["CDR.Services/CDR.Services.csproj", "CDR.Services/"]
COPY ["CDR.Shared/CDR.Shared.csproj", "CDR.Shared/"]
RUN dotnet restore "CDR.API/CDR.API.csproj"

# Copy source code and build
COPY . .
WORKDIR "/build/CDR.API"
RUN dotnet build "CDR.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish artifacts
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CDR.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY CDR.API/Upload /app/Upload

ENV UPLOAD_FOLDER /app/Upload

COPY CDR.API/Setup /app/Setup

ENV SETUP_FOLDER /app/Setup

ENTRYPOINT ["dotnet", "CDR.API.dll"]
