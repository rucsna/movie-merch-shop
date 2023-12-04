FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ./MovieMerchShop.csproj .
RUN dotnet restore "./MovieMerchShop.csproj"

# Copy the entire application code
COPY . .
RUN dotnet build "MovieMerchShop.csproj" -c Release -o /app/build

FROM base AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "MovieMerchShop.dll"]
