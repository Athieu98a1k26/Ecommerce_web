FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/Ecommerce.Web.Host/Ecommerce.Web.Host.csproj", "src/Ecommerce.Web.Host/"]
COPY ["src/Ecommerce.Web.Core/Ecommerce.Web.Core.csproj", "src/Ecommerce.Web.Core/"]
COPY ["src/Ecommerce.Application/Ecommerce.Application.csproj", "src/Ecommerce.Application/"]
COPY ["src/Ecommerce.Core/Ecommerce.Core.csproj", "src/Ecommerce.Core/"]
COPY ["src/Ecommerce.EntityFrameworkCore/Ecommerce.EntityFrameworkCore.csproj", "src/Ecommerce.EntityFrameworkCore/"]
WORKDIR "/src/src/Ecommerce.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/Ecommerce.Web.Host", "src/Ecommerce.Web.Host"]
COPY ["src/Ecommerce.Web.Core", "src/Ecommerce.Web.Core"]
COPY ["src/Ecommerce.Application", "src/Ecommerce.Application"]
COPY ["src/Ecommerce.Core", "src/Ecommerce.Core"]
COPY ["src/Ecommerce.EntityFrameworkCore", "src/Ecommerce.EntityFrameworkCore"]
WORKDIR "/src/src/Ecommerce.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Ecommerce.Web.Host.dll"]
