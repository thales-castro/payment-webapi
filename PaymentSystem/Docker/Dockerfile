#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Source/PaymentSystem.WebApi/PaymentSystem.WebApi.csproj", "Source/PaymentSystem.WebApi/"]
RUN dotnet restore "Source/PaymentSystem.WebApi/PaymentSystem.WebApi.csproj"
COPY . .
WORKDIR "/src/Source/PaymentSystem.WebApi"
RUN dotnet build "PaymentSystem.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentSystem.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# For Heroku Deployment, keep this line commented.
ENTRYPOINT ["dotnet", "PaymentSystem.WebApi.dll"]

# For Heroku Deployment, keep those lines uncommented.
# RUN useradd -m trackersystemuser
# USER trackersystemuser
# CMD ASPNETCORE_URLS="http://*:$PORT" dotnet PaymentSystem.WebApi.dll