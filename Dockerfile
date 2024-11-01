# SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 AS build
WORKDIR /src

# Copy the solution file and individual project files
COPY ["ProductSalesAPI.Domain/ProductSalesAPI.Domain.csproj", "ProductSalesAPI.Domain/"]
COPY ["ProductSalesAPI.Infrastructure/ProductSalesAPI.Infrastructure.csproj", "ProductSalesAPI.Infrastructure/"]
COPY ["ProductSalesAPI.Application/ProductSalesAPI.Application.csproj", "ProductSalesAPI.Application/"]
COPY ["ProductSalesAPI.Presentation/ProductSalesAPI.Presentation.csproj", "ProductSalesAPI.Presentation/"]

# Restore dependencies
RUN dotnet restore "ProductSalesAPI.Presentation/ProductSalesAPI.Presentation.csproj"

# Copy the entire solution directory into the container
COPY ../ .

# Set the working directory to the main project and build
WORKDIR "/src/ProductSalesAPI.Presentation"
RUN dotnet build "ProductSalesAPI.Presentation.csproj" -c Release -o /app/build

# Publish the app to a separate directory
FROM build AS publish
RUN dotnet publish "ProductSalesAPI.Presentation.csproj" -c Release -o /app/publish

# Runtime image for final deployment
FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:6c4df091e4e531bb93bdbfe7e7f0998e7ced344f54426b7e874116a3dc3233ff AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Final stage: copy the published app and run
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductSalesAPI.Presentation.dll"]
