FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["Authentication.App.Challenge.Web/Authentication.App.Challenge.Web.csproj", "Authentication.App.Challenge.Web/"]
RUN dotnet restore "Authentication.App.Challenge.Web/Authentication.App.Challenge.Web.csproj"
COPY . .
WORKDIR "/src/Authentication.App.Challenge.Web"
RUN dotnet build "Authentication.App.Challenge.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authentication.App.Challenge.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication.App.Challenge.Web.dll"]
