# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Set the PORT environment variable
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Use the official ASP.NET Core SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

# Copy the project files and restore dependencies
COPY ["CDR.API/CDR.API.csproj", "CDR.API/"]
COPY ["CDR.Data/CDR.Data.csproj", "CDR.Data/"]
COPY ["CDR.Entities/CDR.Entities.csproj", "CDR.Entities/"]
COPY ["CDR.Services/CDR.Services.csproj", "CDR.Services/"]
COPY ["CDR.Shared/CDR.Shared.csproj", "CDR.Shared/"]
RUN dotnet restore "CDR.API/CDR.API.csproj"

# Copy the rest of the files and build the project
COPY . .
WORKDIR "/build/CDR.API"
RUN dotnet build "CDR.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CDR.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CDR.API.dll"]
