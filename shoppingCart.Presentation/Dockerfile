#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["shoppingCart.Presentation/shoppingCart.Presentation.csproj", "shoppingCart.Presentation/"]
COPY ["shoppingCart.DataAcess/shoppingCart.DataAcess.csproj", "shoppingCart.DataAcess/"]
COPY ["shoppingCart.Entities/shoppingCart.Entities.csproj", "shoppingCart.Entities/"]
COPY ["ShoppingCart.Utilities/ShoppingCart.Utilities.csproj", "ShoppingCart.Utilities/"]
RUN dotnet restore "./shoppingCart.Presentation/./shoppingCart.Presentation.csproj"
COPY . .
WORKDIR "/src/shoppingCart.Presentation"
RUN dotnet build "./shoppingCart.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./shoppingCart.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "shoppingCart.Presentation.dll"]