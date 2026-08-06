# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CoursePilot.Api/CoursePilot.Api.csproj", "CoursePilot.Api/"]
RUN dotnet restore "CoursePilot.Api/CoursePilot.Api.csproj"

COPY . .
WORKDIR "/src/CoursePilot.Api"
RUN dotnet publish "CoursePilot.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "CoursePilot.Api.dll"]