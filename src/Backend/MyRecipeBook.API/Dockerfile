#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["src/Backend/MyRecipeBook.API/MyRecipeBook.API.csproj", "src/Backend/MyRecipeBook.API/"]
COPY ["src/Shared/MyRecipeBook.Communication/MyRecipeBook.Communication.csproj", "src/Shared/MyRecipeBook.Communication/"]
COPY ["src/Shared/MyRecipeBook.Exceptions/MyRecipeBook.Exceptions.csproj", "src/Shared/MyRecipeBook.Exceptions/"]
COPY ["src/Backend/MyRecipeBook.Application/MyRecipeBook.Application.csproj", "src/Backend/MyRecipeBook.Application/"]
COPY ["src/Backend/MyRecipeBook.Domain/MyRecipeBook.Domain.csproj", "src/Backend/MyRecipeBook.Domain/"]
COPY ["src/Backend/MyRecipeBook.Infrastructure/MyRecipeBook.Infrastructure.csproj", "src/Backend/MyRecipeBook.Infrastructure/"]
RUN dotnet restore "./src/Backend/MyRecipeBook.API/MyRecipeBook.API.csproj"
COPY . .
WORKDIR "/src/src/Backend/MyRecipeBook.API"
RUN dotnet build "./MyRecipeBook.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./MyRecipeBook.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyRecipeBook.API.dll"]