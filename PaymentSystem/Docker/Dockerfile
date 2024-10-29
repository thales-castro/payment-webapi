#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Source/PaymentSystem.WebApi/PaymentSystem.WebApi.csproj", "Source/PaymentSystem.WebApi/"]
RUN dotnet restore "Source/PaymentSystem.WebApi/PaymentSystem.WebApi.csproj"
COPY . .
WORKDIR "/src/Source/PaymentSystem.WebApi"
RUN dotnet build "PaymentSystem.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PaymentSystem.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# For Heroku Deployment, keep this line commented.
ENTRYPOINT ["dotnet", "PaymentSystem.WebApi.dll"]

# For Heroku Deployment, keep those lines uncommented.
# RUN useradd -m paymentsystemuser
# USER paymentsystemuser
# CMD ASPNETCORE_URLS="http://*:$PORT" dotnet PaymentSystem.WebApi.dll